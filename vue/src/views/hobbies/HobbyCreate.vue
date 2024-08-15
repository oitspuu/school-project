<script setup lang="ts">
import AccountService from '@/services/AccountService'
import HobbyService from '@/services/HobbyService'
import { useAuthStore } from '@/stores/auth'
import type { IHobbyCreate } from '@/types/IHobbyCreate'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const authStore = useAuthStore()
const router = useRouter()
let validationError = ref<string>('')

const example: IHobbyCreate = {
  timeSpent: '00:00:00',
  hobbyName: '',
  language: null
}

let hobby = ref<IHobbyCreate>(example)

const validateAndCreate = async () => {
  const regexTime = /^(\d*.)?\d{2}:\d{2}:\d{2}$/
  if (!regexTime.test(hobby.value.timeSpent)) {
    validationError.value = 'wrong time format'
    return
  }

  if (hobby.value.hobbyName.trim() == '') {
    validationError.value = 'no hobby name entered'
    return
  }

  if (authStore.jwtInfo) {
    let response = await HobbyService.create(authStore.jwtInfo, hobby.value)
    if (response.status == 201 || response.status == 204) {
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
      response = response = await HobbyService.create(authStore.jwtInfo, hobby.value)
      if (response.status == 201 || response.status == 204) {
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
  <h1>Create</h1>

  <h4>Hobby</h4>
  <hr />
  <div class="row">
    <div class="col-md-4">
      <div>
        <div class="text-danger">{{validationError}}</div>
        <div class="form-group">
          <label htmlFor="hobbyName" class="control-label">Hobby name</label>
          <input id="hobbyName" value="" v-model="hobby.hobbyName" class="form-control" />
        </div>
        <div class="form-group">
          <label htmlFor="homeworkTime" class="control-label">Time spent((d.)hh:mm:ss)</label>
          <input id="homeworkTime" value="" v-model="hobby.timeSpent" class="form-control" />
          <div class="form-group">
            <input type="submit" @click="validateAndCreate" value="Save" class="btn btn-primary" />
          </div>
        </div>
      </div>
    </div>

    <div>
      <router-link to="/hobby">Back to List</router-link>
    </div>
  </div>
</template>
