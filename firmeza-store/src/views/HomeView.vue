<script setup>
import { ref, onMounted } from 'vue';
import { RouterLink } from 'vue-router';
import { productsService } from '../services/productsService';
import ProductCard from '../components/product/ProductCard.vue';

const products = ref([]);
const loading = ref(true);

onMounted(async () => {
  try {
    const data = await productsService.getAll();
    products.value = data.slice(0, 6); // Show only 6 products
  } catch (error) {
    console.error('Error loading products:', error);
  } finally {
    loading.value = false;
  }
});
</script>

<template>
  <div>
    <!-- Hero Section -->
    <div class="bg-indigo-600">
      <div class="max-w-7xl mx-auto py-16 px-4 sm:py-24 sm:px-6 lg:px-8">
        <div class="text-center">
          <h1 class="text-4xl font-extrabold text-white sm:text-5xl sm:tracking-tight lg:text-6xl">
            Bienvenido a Firmeza Store
          </h1>
          <p class="mt-4 max-w-xl mx-auto text-xl text-indigo-100">
            Encuentra los mejores productos al mejor precio
          </p>
          <RouterLink
            to="/products"
            class="mt-8 inline-block bg-white py-3 px-8 border border-transparent rounded-md text-base font-medium text-indigo-600 hover:bg-indigo-50"
          >
            Ver Productos
          </RouterLink>
        </div>
      </div>
    </div>

    <!-- Featured Products -->
    <div class="max-w-7xl mx-auto py-12 px-4 sm:px-6 lg:px-8">
      <h2 class="text-3xl font-bold text-gray-900 mb-8">Productos Destacados</h2>
      
      <div v-if="loading" class="text-center py-12">
        <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
      </div>

      <div v-else class="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3">
        <ProductCard v-for="product in products" :key="product.id" :product="product" />
      </div>

      <div class="mt-8 text-center">
        <RouterLink
          to="/products"
          class="text-indigo-600 hover:text-indigo-500 font-medium"
        >
          Ver todos los productos →
        </RouterLink>
      </div>
    </div>
  </div>
</template>
