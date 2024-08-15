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

let isLoading = ref<boolean>(true)
let validationError = ref<string>('')
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

const deleteHobby = async () => {
  if (!authStore.jwtInfo) {
    validationError.value = "No authentication info, can't delete"
    return
  }
  if (!hobby.value) {
    validationError.value = ' No hobby to delete'
    return
  }
  let response = await HobbyService.deleteHobby(authStore.jwtInfo, hobby.value.id)
  if (response.status == 204) {
    router.push('/hobby')
    return
  }
  if (response.status == 401) {
    const refresh = await AccountService.refresh(authStore.jwtInfo)
    if (!refresh.data) {
      validationError.value = "Not authenticated, can't delete"
      return
    }
    authStore.jwtInfo = refresh.data
    response = await HobbyService.deleteHobby(authStore.jwtInfo, hobby.value.id)
    if (response.status == 204) {
      router.push('/hobby')
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
      <h4>Hobby</h4>
      <hr />
      <div class="text-danger">{{ validationError }}</div>
      <dl class="row">
        <dt class="col-sm-2">Hobby name</dt>
        <dd class="col-sm-10">
          {{ hobby?.hobbyName }}
        </dd>
      </dl>
      <dl class="row">
        <dt class="col-sm-2">Hobby time</dt>
        <dd class="col-sm-10">
          {{ hobby?.timeSpent }}
        </dd>
      </dl>

      <div class="form-group">
        <input type="submit" @click="deleteHobby" value="Delete" class="btn btn-danger" />
      </div>

      <router-link to="/hobby">Back to List</router-link>
    </div>
  </div>
</template>
