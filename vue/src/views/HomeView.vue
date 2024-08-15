<script setup lang="ts">
import CoursesList from '@/components/CoursesList.vue'
import HobbiesList from '@/components/HobbiesList.vue'
import SleepsComponent from '@/components/SleepsComponent.vue'
import WorkplacesList from '@/components/WorkplacesList.vue'
</script>

<template>
  <div className="row">
    <div className="col">
      <courses-list></courses-list>
    </div>
    <div className="col">
      <workplaces-list></workplaces-list>
    </div>
  </div>
  <div className="row">
    <div className="col">
      <sleeps-component></sleeps-component>
    </div>
    <div className="col">
      <hobbies-list></hobbies-list>
    </div>
  </div>
</template>
