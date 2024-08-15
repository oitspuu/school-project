<script setup lang="ts">
import AccountService from '@/services/AccountService'
import SleepService from '@/services/SleepService'
import { useAuthStore } from '@/stores/auth'
import type { ISleep } from '@/types/ISleep'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import SpinnerComponent from '@/components/SpinnerComponent.vue'

const props = defineProps<{
  id: string
  required: true
}>()
const authStore = useAuthStore()
const router = useRouter()
let validationError = ref<string>('')
let isLoading = ref<boolean>(true)
let sleep = ref<ISleep>()

const loadData = async () => {
  if (authStore.jwtInfo) {
    let response = await SleepService.getSleep(authStore.jwtInfo, props.id)
    if (response.data) {
      sleep.value = response.data
      isLoading.value = false
      return
    }

    const refresh = await AccountService.refresh(authStore.jwtInfo)
    if (!refresh.data) return
    authStore.jwtInfo = refresh.data
    response = await SleepService.getSleep(authStore.jwtInfo, props.id)
    if (response.data) {
      sleep.value = response.data
      isLoading.value = false
      return
    }
  }
}
onMounted(() => {
  loadData()
})

const validateAndSave = async () => {
  if (!sleep.value) {
    validationError.value = 'no sleep time to edit'
    return
  }

  const regexTime = /^\d{2}:\d{2}:\d{2}$/
  if (!regexTime.test(sleep.value.start)) {
    validationError.value = 'wrong time format'
    return
  }

  if (!regexTime.test(sleep.value.end)) {
    validationError.value = 'wrong time format'
    return
  }

  const regexDate = /^\d{4}-\d{2}-\d{2}$/
  if (!regexDate.test(sleep.value.day)) {
    validationError.value = 'wrong date format'
    return
  }

  if (authStore.jwtInfo) {
    let response = await SleepService.edit(authStore.jwtInfo, sleep.value)
    if (response.status == 204) {
      router.push('/sleep')
    }

    if (response.status == 401) {
      const refresh = await AccountService.refresh(authStore.jwtInfo)
      if (!refresh.data) {
        validationError.value = refresh.errors![0]
        return
      }
      authStore.jwtInfo = refresh.data
      response = await SleepService.edit(authStore.jwtInfo, sleep.value)
      if (response.status == 204) {
        router.push('/sleep')
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

    <h4>Sleep</h4>
    <hr />
    <div class="row" v-if="sleep">
      <div class="col-md-4">
        <div>
          <div class="text-danger">{{ validationError }}</div>
          <div class="form-group">
            <label htmlFor="day" class="control-label">Day (yyyy-mm-dd)</label>
            <input id="day" value="" v-model="sleep.day" class="form-control" />
          </div>
          <div class="form-group">
            <label htmlFor="start" class="control-label">Start (hh:mm:ss)</label>
            <input id="start" value="" v-model="sleep.start" class="form-control" />
          </div>
          <div class="form-group">
            <label htmlFor="end" class="control-label">End (hh:mm:ss)</label>
            <input id="end" value="" v-model="sleep.end" class="form-control" />
          </div>

          <div class="form-group">
            <input type="submit" @click="validateAndSave" value="Save" class="btn btn-primary" />
          </div>
        </div>
      </div>
    </div>

    <div>
      <router-link to="/sleep">Back to List</router-link>
    </div>
  </div>
</template>
