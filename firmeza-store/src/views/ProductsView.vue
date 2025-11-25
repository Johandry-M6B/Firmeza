<script setup>
import { ref, onMounted } from 'vue';
import { productsService } from '../services/productsService';
import ProductCard from '../components/product/ProductCard.vue';

const products = ref([]);
const loading = ref(true);
const searchTerm = ref('');

onMounted(async () => {
  try {
    const data = await productsService.getAll();
    products.value = data;
  } catch (error) {
    console.error('Error loading products:', error);
  } finally {
    loading.value = false;
  }
});

const filteredProducts = computed(() => {
  if (!searchTerm.value) return products.value;
  return products.value.filter(p => 
    p.name.toLowerCase().includes(searchTerm.value.toLowerCase())
  );
});
</script>

<template>
  <div class="max-w-7xl mx-auto py-8 px-4 sm:px-6 lg:px-8">
    <div class="mb-8">
      <h1 class="text-3xl font-bold text-gray-900">Todos los Productos</h1>
      <div class="mt-4">
        <input
          v-model="searchTerm"
          type="text"
          placeholder="Buscar productos..."
          class="w-full max-w-md px-4 py-2 border border-gray-300 rounded-md focus:ring-indigo-500 focus:border-indigo-500"
        />
      </div>
    </div>

    <div v-if="loading" class="text-center py-12">
      <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
    </div>

    <div v-else-if="filteredProducts.length === 0" class="text-center py-12">
      <p class="text-gray-500">No se encontraron productos</p>
    </div>

    <div v-else class="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
      <ProductCard v-for="product in filteredProducts" :key="product.id" :product="product" />
    </div>
  </div>
</template>

<script>
import { computed } from 'vue';
export default {
  name: 'ProductsView'
};
</script>
