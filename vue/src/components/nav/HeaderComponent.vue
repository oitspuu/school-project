<script setup lang="ts">
import IdentityComponent from './IdentityComponent.vue'
</script>

<template>
  <header>
    <nav
      class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3"
    >
      <div class="container">
        <router-link to="/" class="navbar-brand">Personal Time Tracker</router-link>
        <button
          class="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target=".navbar-collapse"
          aria-controls="navbarSupportedContent"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
          <ul class="navbar-nav flex-grow-1">
            <li class="nav-item">
              <router-link class="nav-link text-dark" to="/">Home</router-link>
            </li>
            <li class="nav-item">
              <router-link to="/course" class="nav-link text-dark">Course</router-link>
            </li>
            <li class="nav-item">
              <router-link to="/sleep" class="nav-link text-dark">Sleep</router-link>
            </li>
            <li class="nav-item">
              <router-link to="/hobby" class="nav-link text-dark">Hobby</router-link>
            </li>
            <li class="nav-item">
              <router-link to="/work" class="nav-link text-dark">Work</router-link>
            </li>
          </ul>
          <identity-component> </identity-component>
        </div>
      </div>
    </nav>
  </header>
</template>

<style scoped></style>
