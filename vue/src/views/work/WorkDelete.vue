<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'
import type { IWork } from '@/types/IWork'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import SpinnerComponent from '@/components/SpinnerComponent.vue'
import AccountService from '@/services/AccountService'
import WorkService from '@/services/WorkService'

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

const deleteWork = async () => {
  if (!authStore.jwtInfo) {
    validationError.value = "No authentication info, can't delete"
    return
  }
  let response = await WorkService.deleteWork(authStore.jwtInfo, props.id)
  if (response.status == 204) {
    router.push('/work')
    return
  }
  if (response.status == 401) {
    const refresh = await AccountService.refresh(authStore.jwtInfo)
    if (!refresh.data) {
      validationError.value = "Not authenticated, can't delete"
      return
    }
    authStore.jwtInfo = refresh.data
    response = await WorkService.deleteWork(authStore.jwtInfo, props.id)
    if (response.status == 204) {
      router.push('/work')
      return
    }
  }

  if (response.errors && response.errors.length > 0) {
    validationError.value = response.errors[0]
  } else {
    validationError.value = 'unknown error'
  }
}
</script>
<template>
  <div v-if="isLoading">
    <spinner-component></spinner-component>
  </div>
  <div v-else>
    <h1>Delete</h1>

    <h3>Are you sure you want to delete this?</h3>
    <div>
      <h4>Work</h4>
      <hr />
      <div class="text-danger">{{ validationError }}</div>
      <dl class="row">
        <dt class="col-sm-2">Work name</dt>
        <dd class="col-sm-10">
          {{ work?.workName }}
        </dd>
      </dl>
      <dl class="row">
        <dt class="col-sm-2">Total time</dt>
        <dd class="col-sm-10">
          {{ work?.totalWorkHours }}
        </dd>
      </dl>

      <div class="form-group">
        <input type="submit" @click="deleteWork" value="Delete" class="btn btn-danger" />
      </div>

      <router-link to="/work">Back to List</router-link>
    </div>
  </div>
</template>
