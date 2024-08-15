<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'
import AccountService from '@/services/AccountService'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const authStore = useAuthStore()
const router = useRouter()

let email = ref<string>('')
let pwd = ref<string>('')
let validationError = ref<string>('')

const validateAndLogin = async () => {
  if (email.value.length < 5 || pwd.value.length < 6) {
    validationError.value = 'Invalid input lengths'
    return
  }

  const response = await AccountService.login(email.value, pwd.value)
  if (response.data) {
    validationError.value = ''
    authStore.userName = email.value
    authStore.jwtInfo = response.data
    const url = authStore.returnUrl ?? '/';
    authStore.returnUrl = null;
    router.push(url)
    return;
  }

  if (response.errors && response.errors.length > 0) {
    validationError.value = response.errors[0]
  }
  return
}
</script>

<template>
  <div class="d-flex flex-column align-items-center">
    <h1>Login page</h1>
    <div>
      <div class="row">
        <div class="col">
          <section>
            <div class="text-danger" role="alert">{{ validationError }}</div>
            <div class="form-floating mb-3">
              <input
                v-model="email"
                id="email"
                value=""
                class="form-control"
                type="email"
                aria-required="true"
                placeholder="name@example.com"
              />
              <label htmlFor="email" class="form-label">Email</label>
            </div>
            <div class="form-floating mb-3">
              <input
                id="password"
                v-model="pwd"
                value=""
                class="form-control"
                type="password"
                aria-required="true"
                placeholder="password"
              />
              <label htmlFor="password" class="form-label">Password</label>
            </div>
            <div class="form-floating mb-3">
              <button
                id="login-submit"
                @click="validateAndLogin"
                class="w-100 btn btn-lg btn-primary">
                Log in
              </button>
            </div>
            <div class="form-floating mb-3">
              <RouterLink to="/register" class="w-100 btn btn-lg btn-primary">Register</RouterLink>
            </div>
          </section>
        </div>
      </div>
    </div>
  </div>
</template>
