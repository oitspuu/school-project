<script setup lang="ts">
import AccountService from '@/services/AccountService'
import HobbyService from '@/services/HobbyService'
import { useAuthStore } from '@/stores/auth'
import type { IAddTime } from '@/types/IAddTime'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const props = defineProps<{
  id: string
  required: true
}>()

const authStore = useAuthStore()
const router = useRouter()
let validationError = ref<string>('')

const example: IAddTime = {
  id: props.id,
  timeSpent: '00:00:00'
}

let time = ref<IAddTime>(example)

const validateAndSave = async () => {
  if (time.value.timeSpent === '00:00:00') {
    router.push('/hobby')
  }

  const regexTime = /^(\d*.)?\d{2}:\d{2}:\d{2}$/
  if (!regexTime.test(time.value.timeSpent)) {
    validationError.value = 'wrong time format'
    return
  }

  if (authStore.jwtInfo) {
    let response = await HobbyService.addTime(authStore.jwtInfo, time.value.id, time.value)
    if (response.status == 204) {
      router.push('/hobby')
      return;
    }

    if (response.status == 401) {
      const refresh = await AccountService.refresh(authStore.jwtInfo)
      if (!refresh.data) {
        validationError.value = refresh.errors![0]
        return
      }
      authStore.jwtInfo = refresh.data
      response = await HobbyService.addTime(authStore.jwtInfo, time.value.id, time.value)
      if (response.status == 204) {
        router.push('/hobby')
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
  <h1>Add time to total time spent</h1>

  <h4>Hobby</h4>
  <hr />
  <div class="row">
    <div class="col-md-4">
      <div>
        <div class="form-group">
          <label htmlFor="timeSpent" class="control-label">Add time</label>
          <input id="timeSpent" class="form-control" value="" v-model="time.timeSpent" />
          <span class="text-danger">{{ validationError }}</span>
        </div>

        <div class="form-group">
          <input type="submit" @click="validateAndSave" value="Add" class="btn btn-primary" />
        </div>
      </div>
    </div>
  </div>

  <div>
    <router-link to="/hobby">Back to List</router-link>
  </div>
</template>
