<script setup lang="ts">
import AccountService from '@/services/AccountService'
import WorkService from '@/services/WorkService'
import { useAuthStore } from '@/stores/auth'
import type { IWork } from '@/types/IWork'
import { onMounted, ref } from 'vue'
import SpinnerComponent from '@/components/SpinnerComponent.vue'
import WorkHoursList from '@/components/WorkHoursList.vue'

const props = defineProps<{
  id: string
  required: true
}>()

const authStore = useAuthStore()
let isLoading = ref<boolean>(true)
let work = ref<IWork>()

authStore.workId = props.id

const loadData = async () => {
  if (authStore.jwtInfo) {
    let response = await WorkService.getWork(authStore.jwtInfo, props.id)
    if (response.data) {
      work.value = response.data
      isLoading.value = false
      return
    }

    const refresh = await AccountService.refresh(authStore.jwtInfo)
    if (!refresh.data) return
    authStore.jwtInfo = refresh.data
    response = await WorkService.getWork(refresh.data, props.id)
    if (response.data) {
      work.value = response.data
      isLoading.value = false
      return
    }
  }
}

onMounted(() => {
  loadData()
})
</script>
<template>
  <h1>Details</h1>
  <div v-if="isLoading">
    <spinner-component></spinner-component>
  </div>
  <h1>Details</h1>
  <div class="row">
    <div class="col">
      <dl>
        <dt class="col-sm-2">Work name</dt>
        <dd class="col-sm-10">
          {{ work?.workName }}
        </dd>
        <dt class="col-sm-2">Start date</dt>
        <dd class="col-sm-10">
          {{ work?.start }}
        </dd>
        <dt class="col-sm-2">End date</dt>
        <dd class="col-sm-10">
          {{ work?.end }}
        </dd>
        <dt class="col-sm-2">Lunch break duration</dt>
        <dd class="col-sm-10">
          {{ work?.lunchBreakDuration }}
        </dd>
        <dt class="col-sm-2">Total hours</dt>
        <dd class="col-sm-10">
          {{ work?.totalWorkHours }}
        </dd>
      </dl>
      <div>
        <router-link :to="{ name: 'workedit', params: { id: props.id } }" class="btn btn-primary"
          >Edit</router-link
        >
        <router-link to="/work" class="btn btn-primary">Back to List</router-link>
      </div>
      <work-hours-list></work-hours-list>
    </div>
  </div>
</template>
