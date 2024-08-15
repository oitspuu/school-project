<script setup lang="ts">
import AccountService from '@/services/AccountService'
import WorkHoursService from '@/services/WorkHoursService'
import { useAuthStore } from '@/stores/auth'
import type { IWorkHoursCreate } from '@/types/IWorkHoursCreate'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const props = defineProps<{
  id: string
  required: true
}>()

const authStore = useAuthStore()
const router = useRouter()
let validationError = ref<string>('')

const example: IWorkHoursCreate = {
  userWorkId: props.id,
  date: 'yyyy-mm-dd',
  startTime: '00:00:00',
  endTime: '00:00:00',
  lunchBreak: false
}

let workhours = ref<IWorkHoursCreate>(example)

const validateAndCreate = async () => {
  const regexTime = /^\d{2}:\d{2}:\d{2}$/
  if (!regexTime.test(workhours.value.startTime)) {
    validationError.value = 'wrong time format'
    return
  }

  if (!regexTime.test(workhours.value.endTime)) {
    validationError.value = 'wrong time format'
    return
  }

  const regexDate = /^\d{4}-\d{2}-\d{2}$/
  if (!regexDate.test(workhours.value.date)) {
    validationError.value = 'wrong date format'
    return
  }

  if (authStore.jwtInfo) {
    let response = await WorkHoursService.create(authStore.jwtInfo, workhours.value)
    if (response.status == 201 || response.status == 204) {
      router.push({ name: 'workdetails', params: { id: workhours.value.userWorkId } }).catch(() => {});
      return;
    }

    if (response.status == 401) {
      const refresh = await AccountService.refresh(authStore.jwtInfo)
      if (!refresh.data) {
        validationError.value = refresh.errors![0]
        return
      }
      authStore.jwtInfo = refresh.data
      response = response = await WorkHoursService.create(authStore.jwtInfo, workhours.value)
      if (response.status == 201 || response.status == 204) {
        router.push({ name: 'workdetails', params: { id: workhours.value.userWorkId } }).catch(() => {});
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
  <h1>Add new day</h1>

  <hr />
  <div class="row">
    <div class="col-md-4">
      <div>
        <div class="text-danger">{{ validationError }}</div>
        <div class="form-group">
          <label htmlFor="courseName" class="control-label">Date (yyyy-mm-dd)</label>
          <input id="courseName" value="" v-model="workhours.date" class="form-control" />
        </div>
        <div class="form-group">
          <label htmlFor="homeworkTime" class="control-label">Start(hh:mm:ss)</label>
          <input id="homeworkTime" value="" v-model="workhours.startTime" class="form-control" />
        </div>
        <div class="form-group">
          <label htmlFor="homeworkTime" class="control-label">End(hh:mm:ss)</label>
          <input id="homeworkTime" value="" v-model="workhours.endTime" class="form-control" />
        </div>
        <div class="form-group form-check">
          <label class="form-check-label" htmlFor="lunch">
            <input
              id="lunch"
              value="false"
              type="checkbox"
              v-model="workhours.lunchBreak"
              class="form-check-input"
            />
          </label>
        </div>
        <div class="form-group">
          <input type="submit" @click="validateAndCreate" value="Save" class="btn btn-primary" />
        </div>
      </div>
    </div>

    <div>
      <router-link :to="{ name: 'workdetails', params: { id: props.id } }"
        >Back to workhours list</router-link
      >
    </div>
  </div>
</template>
