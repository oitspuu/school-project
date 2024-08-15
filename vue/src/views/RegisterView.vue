<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'
import AccountService from '@/services/AccountService'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const authStore = useAuthStore()
const router = useRouter()

let email = ref<string>('')
let pwd = ref<string>('')
let passwordConfirm = ref<string>('')
let validationError = ref<string>('')

const regex = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W]).{6,}$/

const validateAndRegister = async () => {
  if (email.value.length < 5 || pwd.value.length < 6) {
    validationError.value = 'Invalid input lengths'
    return
  }

  if (pwd.value !== passwordConfirm.value) {
    validationError.value = "Passwords don't match"
    return
  }

  // eslint-disable-next-line no-useless-escape
  if (!email.value.match('[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$')) {
    validationError.value = 'Invalid email'
    return
  }

  if (!regex.test(pwd.value)) {
    validationError.value =
      'Password must contain atleast 1 number, uppercase letter, lowecase letter and a special character (e.g !,?)'
    return
  }

  const response = await AccountService.register(email.value, pwd.value)
  if (response.data) {
    validationError.value = ''
    authStore.userName = email.value
    authStore.jwtInfo = response.data
    router.push('/')
    return
  }

  if (response.errors && response.errors.length > 0) {
    validationError.value = response.errors[0]
  }
}
</script>

<template>
  <div class="d-flex flex-column align-items-center">
    <h1>Register new user</h1>
    <div class="row">
      <div class="col">
        <h2>Create a new account.</h2>
        <hr />
        <div class="text-danger" role="alert">{{ validationError }}</div>
        <div class="form-floating mb-3">
          <input
            id="email"
            v-model="email"
            value=""
            type="email"
            class="form-control"
            aria-required="true"
            placeholder="name@example.com"
          />
          <label htmlFor="email">Email</label>
        </div>
        <div class="form-floating mb-3">
          <input
            id="password"
            v-model="pwd"
            value=""
            type="password"
            class="form-control"
            aria-required="true"
            placeholder="password"
          />
          <label htmlFor="password">Password</label>
        </div>
        <div class="form-floating mb-3">
          <input
            id="passwordConfirm"
            v-model="passwordConfirm"
            value=""
            type="password"
            class="form-control"
            aria-required="true"
            placeholder="password"
          />
          <label htmlFor="passwordConfirm">Confirm Password</label>
        </div>
        <button
          id="registerSubmit"
          @click="validateAndRegister"
          class="w-100 btn btn-lg btn-primary"
        >
          Register
        </button>
      </div>
    </div>
  </div>
</template>
