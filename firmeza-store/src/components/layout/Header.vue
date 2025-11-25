<script setup>
import { RouterLink } from 'vue-router';
import { useAuthStore } from '../../stores/authStore';
import { useCartStore } from '../../stores/cartStore';
import { ShoppingCartIcon, UserIcon } from '@heroicons/vue/24/outline';

const authStore = useAuthStore();
const cartStore = useCartStore();
</script>

<template>
  <header class="bg-white shadow">
    <nav class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex justify-between h-16">
        <div class="flex">
          <RouterLink to="/" class="flex items-center text-xl font-bold text-indigo-600">
            Firmeza Store
          </RouterLink>
          <div class="hidden sm:ml-6 sm:flex sm:space-x-8">
            <RouterLink to="/" class="inline-flex items-center px-1 pt-1 text-sm font-medium text-gray-900">
              Inicio
            </RouterLink>
            <RouterLink to="/products" class="inline-flex items-center px-1 pt-1 text-sm font-medium text-gray-500 hover:text-gray-900">
              Productos
            </RouterLink>
          </div>
        </div>
        <div class="flex items-center space-x-4">
          <RouterLink to="/cart" class="relative p-2 text-gray-400 hover:text-gray-500">
            <ShoppingCartIcon class="h-6 w-6" />
            <span v-if="cartStore.itemCount > 0" class="absolute top-0 right-0 inline-flex items-center justify-center px-2 py-1 text-xs font-bold leading-none text-white transform translate-x-1/2 -translate-y-1/2 bg-red-600 rounded-full">
              {{ cartStore.itemCount }}
            </span>
          </RouterLink>
          <RouterLink v-if="!authStore.isAuthenticated" to="/login" class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700">
            Iniciar Sesión
          </RouterLink>
          <div v-else class="flex items-center space-x-2">
            <RouterLink to="/orders" class="text-sm text-gray-700 hover:text-gray-900">
              Mis Órdenes
            </RouterLink>
            <button @click="authStore.logout" class="text-sm text-gray-500 hover:text-gray-700">
              Salir
            </button>
          </div>
        </div>
      </div>
    </nav>
  </header>
</template>
