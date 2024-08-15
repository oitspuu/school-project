<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'
import type { IWork } from '@/types/IWork'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import SpinnerComponent from '@/components/SpinnerComponent.vue'
import WorkService from '@/services/WorkService'
import AccountService from '@/services/AccountService'

const props = defineProps<{
  id: string
  required: true
}>()

const authStore = useAuthStore()
const router = useRouter()
let validationError = ref<string>('')
let isLoading = ref<boolean>(true)
let work = ref<IWork>()

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

const validateAndSave = async () => {
  if (!work.value) {
    validationError.value = 'no work.value to edit'
    return
  }

  const regexDate = /^\d{4}-\d{2}-\d{2}$/
  if (!regexDate.test(work.value!.end) || !regexDate.test(work.value!.start)) {
    validationError.value = 'wrong date format'
    return
  }

  const regexTime = /^\d{2}:\d{2}:\d{2}$/
  if (!regexTime.test(work.value!.lunchBreakDuration)) {
    validationError.value = 'wrong time format'
    return
  }

  if (work.value!.workName.trim() == '') {
    validationError.value = 'no coursename entered'
    return
  }

  if (authStore.jwtInfo) {
    let response = await WorkService.edit(authStore.jwtInfo, work.value)
    if (response.status == 204) {
      router.push({ name: 'workdetails', params: { id: response.data?.id } }).catch(() => {});
    }

    if (response.status == 401) {
      const refresh = await AccountService.refresh(authStore.jwtInfo)
      if (!refresh.data) {
        validationError.value = refresh.errors![0]
        return
      }
      authStore.jwtInfo = refresh.data
      response = await WorkService.edit(refresh.data, work.value)
      if (response.status == 204) {
        router.push({ name: 'workdetails', params: { id: response.data?.id } }).catch(() => {});
      }
    }
    if (response.errors && response.errors.length > 0) {
      validationError.value = response.errors[0]
    } else {
      validationError.value = 'unknown error'
    }
    return
  }
  validationError.value = 'Not authenticated'
}
</script>
<template>
  <div v-if="isLoading">
    <spinner-component></spinner-component>
  </div>
  <div v-else>
    <h1>Edit</h1>

    <h4>Work</h4>
    <hr />
    <div class="row" v-if="work">
      <div class="col-md-4">
        <div>
          <div class="text-danger">{{validationError}}</div>
          <div class="form-group">
            <label htmlFor="workName" class="control-label">Work name</label>
            <input id="workName" value="" v-model="work.workName" class="form-control" />
          </div>
          <div class="form-group">
            <label htmlFor="lunch" class="control-label">Lunch break duration (hh:mm:ss)</label>
            <input id="lunch" value="" v-model="work.lunchBreakDuration" class="form-control" />
          </div>
          <div class="form-group">
            <label htmlFor="start" class="control-label">Start date (yyyy-mm-dd)</label>
            <input id="start" value="" v-model="work.start" class="form-control" />
          </div>
          <div class="form-group">
            <label htmlFor="end" class="control-label">End date (yyyy-mm-dd)</label>
            <input id="end" value="" v-model="work.end" class="form-control" />
          </div>
          <div class="form-group">
            <input type="submit" @click="validateAndSave" value="Save" class="btn btn-primary" />
          </div>
        </div>
      </div>
    </div>
    <div>
      <router-link to="/work">Back to List</router-link>
    </div>
  </div>
</template>
