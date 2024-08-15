<script setup lang="ts">
import AccountService from '@/services/AccountService'
import WorkService from '@/services/WorkService'
import { useAuthStore } from '@/stores/auth'
import type { IWorkCreate } from '@/types/IWorkCreate'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const authStore = useAuthStore()
const router = useRouter()
let validationError = ref<string>('')

const example: IWorkCreate = {
  workName: '',
  lunchBreakDuration: '00:00:00',
  start: 'yyyy-mm-dd',
  end: 'yyyy-mm-dd'
}

let work = ref<IWorkCreate>(example)

const validateAndCreate = async () => {
  const regexDate = /^\d{4}-\d{2}-\d{2}$/
  if (!regexDate.test(work.value.end)) {
    validationError.value = 'wrong date format'
    return
  }
  if (!regexDate.test(work.value.start)) {
    validationError.value = 'wrong date format'
    return
  }

  const regexTime = /^\d{2}:\d{2}:\d{2}$/
  if (!regexTime.test(work.value.lunchBreakDuration)) {
    validationError.value = 'wrong time format'
    return
  }

  if (work.value.workName.trim() == '') {
    validationError.value = 'no name entered'
    return
  }

  if (authStore.jwtInfo) {
    let response = await WorkService.create(authStore.jwtInfo, work.value)
    if (response.status == 201) {
      router.push('/work')
      return;
    }

    if (response.status == 204) {
      router.push('/work')
      return;
    }

    if (response.status == 401) {
      const refresh = await AccountService.refresh(authStore.jwtInfo)
      if (!refresh.data) {
        validationError.value = refresh.errors![0]
        return
      }
      authStore.jwtInfo = refresh.data
      response = await WorkService.create(refresh.data, work.value)
      if (response.status == 201) {
        router.push('/work')
        return;
      }
      if (response.status == 204) {
        router.push('/work')
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

  <h4>Work</h4>
  <hr />
  <div class="row">
    <div class="col-md-4">
      <div>
        <div class="text-danger">{{ validationError }}</div>
        <div class="form-group">
          <label htmlFor="name" class="control-label">Work name</label>
          <input id="name" value="" v-model="work.workName" class="form-control" />
        </div>
        <div class="form-group">
          <label htmlFor="lunch" class="control-label">Lunch break duration (hh:mm:ss)</label>
          <input id="lunch" value="" v-model="work.lunchBreakDuration" class="form-control" />
        </div>
        <div class="form-group">
          <label htmlFor="stat" class="control-label">Start date (yyyy-mm-dd)</label>
          <input id="start" value="" v-model="work.start" class="form-control" />
        </div>
        <div class="form-group">
          <label htmlFor="end" class="control-label">End date (yyyy-mm-dd)</label>
          <input id="end" value="" v-model="work.end" class="form-control" />
        </div>
        <div class="form-group">
          <input type="submit" @click="validateAndCreate" value="Save" class="btn btn-primary" />
        </div>
      </div>
    </div>
  </div>

  <div>
    <router-link to="/work">Back to List</router-link>
  </div>
</template>
