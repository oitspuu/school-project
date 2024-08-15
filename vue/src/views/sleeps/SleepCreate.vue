<script setup lang="ts">
import AccountService from '@/services/AccountService'
import SleepService from '@/services/SleepService'
import { useAuthStore } from '@/stores/auth'
import type { ISleepCreate } from '@/types/ISleepCreate'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const authStore = useAuthStore()
const router = useRouter()
let validationError = ref<string>('')
const example: ISleepCreate = {
  day: 'yyyy-mm-dd',
  start: '00:00:00',
  end: '00:00:00'
}
let sleep = ref<ISleepCreate>(example)

const validateAndCreate = async () => {
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
    let response = await SleepService.create(authStore.jwtInfo, sleep.value)
    if (response.status == 201 || response.status == 204) {
      router.push('/sleep')
      return;
    }

    if (response.status == 401) {
      const refresh = await AccountService.refresh(authStore.jwtInfo)
      if (!refresh.data) {
        validationError.value = refresh.errors![0]
        return
      }
      authStore.jwtInfo = refresh.data
      response = response = await SleepService.create(authStore.jwtInfo, sleep.value)
      if (response.status == 201 || response.status == 204) {
        router.push('/sleep')
        return;
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
  <h1>Create</h1>

  <h4>Sleep</h4>
  <hr />
  <div class="row">
    <div class="col-md-4">
      <div>
        <div class="text-danger">{{validationError}}</div>
        <div class="form-group">
          <label htmlFor="day" class="control-label">Day</label>
          <input id="day" value="" v-model="sleep.day" class="form-control" />
        </div>
        <div class="form-group">
          <label htmlFor="start" class="control-label">Start(hh:mm:ss)</label>
          <input id="start" value="" v-model="sleep.start" class="form-control" />
        </div>
        <div class="form-group">
          <label htmlFor="end" class="control-label">End(hh:mm:ss)</label>
          <input id="end" value="" v-model="sleep.end" class="form-control" />
        </div>
        <div class="form-group">
          <input
            type="submit"
            @click="validateAndCreate"
            value="Save"
            class="btn btn-primary"
          />
        </div>
      </div>
    </div>

    <div>
      <router-link to="/sleep">Back to List</router-link>
    </div>
  </div>
</template>
