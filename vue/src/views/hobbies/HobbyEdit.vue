<script setup lang="ts">
import AccountService from '@/services/AccountService'
import HobbyService from '@/services/HobbyService'
import { useAuthStore } from '@/stores/auth'
import type { IHobby } from '@/types/IHobby'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import SpinnerComponent from '@/components/SpinnerComponent.vue'

const props = defineProps({
  id: String
})

const authStore = useAuthStore()
const router = useRouter()
let validationError = ref<string>('')
let isLoading = ref<boolean>(true)

let hobby = ref<IHobby>()

const loadData = async () => {
  if (authStore.jwtInfo && props.id) {
    let response = await HobbyService.getHobby(authStore.jwtInfo, props.id)
    if (response.data) {
      hobby.value = response.data
      isLoading.value = false
      return
    }
    const refresh = await AccountService.refresh(authStore.jwtInfo)
    if (!refresh.data) return
    authStore.jwtInfo = refresh.data
    response = await HobbyService.getHobby(refresh.data, props.id)
    if (response.data) {
      hobby.value = response.data
      isLoading.value = false
      return
    }
  }
}

onMounted(() => {
  loadData()
})

const validateAndSave = async () => {
  if (!hobby.value) {
    validationError.value = 'no course to edit'
    return
  }

  const regexTime = /^(\d*.)?\d{2}:\d{2}:\d{2}$/
  if (!regexTime.test(hobby.value.timeSpent)) {
    validationError.value = 'wrong time format'
    return
  }

  if (hobby.value.hobbyName.trim() == '') {
    validationError.value = 'no coursename entered'
    return
  }

  if (authStore.jwtInfo) {
    let response = await HobbyService.edit(authStore.jwtInfo, hobby.value)
    if (response.status == 204) {
      router.push('/hobby')
    }

    if (response.status == 401) {
      const refresh = await AccountService.refresh(authStore.jwtInfo)
      if (!refresh.data) {
        validationError.value = refresh.errors![0]
        return
      }
      authStore.jwtInfo = refresh.data
      response = await HobbyService.edit(authStore.jwtInfo, hobby.value)
      if (response.status == 204) {
        router.push('/hobby')
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

    <h4>Hobby</h4>
    <hr />
    <div class="row" v-if="hobby">
      <div class="col-md-4">
        <div>
          <div class="text-danger">{{ validationError }}</div>
          <div class="form-group">
            <label htmlFor="hobbyName" class="control-label">Hobby name</label>
            <input id="hobbyName" value="" v-model="hobby.hobbyName" class="form-control" />
          </div>
          <div class="form-group">
            <label htmlFor="homeworkTime" class="control-label">Time ((d.)hh:mm:ss)</label>
            <input id="homeworkTime" value="" v-model="hobby.timeSpent" class="form-control" />
          </div>

          <div class="form-group">
            <input
              type="submit"
              @click="validateAndSave"
              value="Save"
              class="btn btn-primary"
            />
          </div>
        </div>
      </div>
    </div>

    <div>
      <router-link to="/hobby">Back to List</router-link>
    </div>
  </div>
</template>
