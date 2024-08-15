<script setup lang="ts">
import { useRouter } from 'vue-router'
import AccountService from '@/services/AccountService'
import { useAuthStore } from '@/stores/auth'
import type { ILogout } from '@/types/ILogout'


const authStore = useAuthStore()
const router = useRouter()

const doLogout = async () => {
  if (authStore.jwtInfo) {
    const token: ILogout = { refreshToken: authStore.jwtInfo.refreshToken }
    await AccountService.logout(token)
    router.push("/login")
  }
  authStore.jwtInfo = null
}
</script>

<template>
  <ul v-if="authStore.isAuthenticated" class="navbar-nav">
    <li class="nav-item">
      <router-link to="/" class="nav-link text-dark" title="Manage"
        >Hello {{ authStore.userName }}</router-link
      >
    </li>
    <li class="nav-item">
      <router-link to="/" @click="doLogout" class="nav-link text-dark" title="Logout"
        >Logout</router-link
      >
    </li>
  </ul>
  <ul v-else class="navbar-nav">
    <li class="nav-item">
      <router-link to="/register" class="nav-link text-dark">Register</router-link>
    </li>
    <li class="nav-item">
      <router-link :to="{ name: 'login' }" class="nav-link text-dark">Login</router-link>
    </li>
  </ul>
</template>
