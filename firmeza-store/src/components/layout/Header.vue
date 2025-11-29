<script setup>
import { ref } from 'vue';
import { RouterLink, useRouter } from 'vue-router';
import { useAuthStore } from '../../stores/authStore';
import { useCartStore } from '../../stores/cartStore';
import { ShoppingCartIcon, MagnifyingGlassIcon, Bars3Icon } from '@heroicons/vue/24/outline';

const authStore = useAuthStore();
const cartStore = useCartStore();
const router = useRouter();

const searchQuery = ref('');
const selectedCategory = ref('all');

const handleSearch = () => {
  if (searchQuery.value.trim()) {
    router.push({ path: '/products', query: { search: searchQuery.value } });
  }
};
</script>

<template>
  <!-- Top Bar -->
  <div class="bg-gray-800 text-white">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-2">
      <div class="flex justify-between items-center text-sm">
        <div class="flex items-center space-x-4">
          <span>📍 Productos</span>
          <span>🛒 Carrito</span>
          <span v-if="authStore.isAuthenticated">👤 Admin</span>
        </div>
        <div class="flex items-center space-x-4">
          <RouterLink v-if="!authStore.isAuthenticated" to="/login" class="hover:text-gray-300">
            Iniciar sesión
          </RouterLink>
          <RouterLink v-if="!authStore.isAuthenticated" to="/register" class="hover:text-gray-300">
            Registrarse
          </RouterLink>
          <button v-if="authStore.isAuthenticated" @click="authStore.logout" class="hover:text-gray-300">
            Salir
          </button>
        </div>
      </div>
    </div>
  </div>

  <!-- Main Header -->
  <header class="bg-blue-600 shadow-lg">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
      <div class="flex items-center justify-between">
        <!-- Logo -->
        <RouterLink to="/" class="flex items-center space-x-2">
          <div class="bg-white p-2 rounded">
            <svg class="h-8 w-8 text-blue-600" fill="currentColor" viewBox="0 0 24 24">
              <path d="M12 2L2 7v10c0 5.55 3.84 10.74 9 12 5.16-1.26 9-6.45 9-12V7l-10-5z"/>
            </svg>
          </div>
          <div class="text-white">
            <div class="text-2xl font-bold">Firmeza</div>
            <div class="text-xs">Tienda Online</div>
          </div>
        </RouterLink>

        <!-- Search Bar -->
        <div class="flex-1 max-w-2xl mx-8">
          <form @submit.prevent="handleSearch" class="flex">
            <input
              v-model="searchQuery"
              type="text"
              placeholder="Buscar productos..."
              class="flex-1 px-4 py-3 rounded-l-md text-gray-900 focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
            <select
              v-model="selectedCategory"
              class="px-4 py-3 bg-white text-gray-900 border-l border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              <option value="all">Todas las categorías</option>
              <option value="alimentos">Alimentos</option>
              <option value="bebidas">Bebidas</option>
              <option value="electronica">Electrónica</option>
              <option value="ferreteria">Ferretería</option>
              <option value="limpieza">Limpieza</option>
            </select>
            <button
              type="submit"
              class="px-6 py-3 bg-blue-700 text-white rounded-r-md hover:bg-blue-800 focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              <MagnifyingGlassIcon class="h-5 w-5" />
            </button>
          </form>
        </div>

        <!-- Cart -->
        <RouterLink to="/cart" class="relative p-3 text-white hover:bg-blue-700 rounded-md transition">
          <ShoppingCartIcon class="h-6 w-6" />
          <span v-if="cartStore.itemCount > 0" class="absolute -top-1 -right-1 inline-flex items-center justify-center px-2 py-1 text-xs font-bold leading-none text-white bg-red-600 rounded-full">
            {{ cartStore.itemCount }}
          </span>
        </RouterLink>
      </div>
    </div>
  </header>
</template>
