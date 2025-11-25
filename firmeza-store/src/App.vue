<script setup>
import { RouterView } from 'vue-router';
import Header from './components/layout/Header.vue';
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <Header />
    <main>
      <RouterView />
    </main>
  </div>
</template>
