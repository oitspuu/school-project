import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import type { IJwtResponse } from '@/types/IJwtResponse'

export const useAuthStore = defineStore('auth', () => {
    // ref - state variables
    const jwtInfo = ref<IJwtResponse | null>(null)
    const userName = ref<string | null>(null)
    const returnUrl = ref<string | null>(null)
    const eventId = ref<string | null>(null)

    // computed - getters
    const isAuthenticated = computed<boolean>(() => !!jwtInfo.value);

    // functions - actions


    // return your refs, computeds and functions
    return { jwtInfo, userName, isAuthenticated, returnUrl, eventId }
})
