<script setup>
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { productsService } from '../services/productsService';
import { useCartStore } from '../stores/cartStore';

const route = useRoute();
const router = useRouter();
const cartStore = useCartStore();

const product = ref(null);
const loading = ref(true);
const quantity = ref(1);

onMounted(async () => {
  try {
    const data = await productsService.getById(route.params.id);
    product.value = data;
  } catch (error) {
    console.error('Error loading product:', error);
    router.push('/products');
  } finally {
    loading.value = false;
  }
});

const addToCart = () => {
  if (product.value) {
    cartStore.addItem(product.value, quantity.value);
    router.push('/cart');
  }
};
</script>

<template>
  <div class="max-w-7xl mx-auto py-8 px-4 sm:px-6 lg:px-8">
    <div v-if="loading" class="text-center py-12">
      <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
    </div>

    <div v-else-if="product" class="grid grid-cols-1 lg:grid-cols-2 gap-8">
      <div class="bg-gray-100 rounded-lg flex items-center justify-center h-96">
        <span class="text-gray-400 text-6xl">📦</span>
      </div>

      <div>
        <h1 class="text-3xl font-bold text-gray-900">{{ product.name }}</h1>
        <p class="mt-4 text-gray-600">{{ product.description }}</p>
        
        <div class="mt-6">
          <span class="text-4xl font-bold text-indigo-600">${{ product.salePrice?.toFixed(2) }}</span>
        </div>

        <div class="mt-8">
          <label class="block text-sm font-medium text-gray-700 mb-2">Cantidad</label>
          <input
            v-model.number="quantity"
            type="number"
            min="1"
            class="w-24 px-3 py-2 border border-gray-300 rounded-md"
          />
        </div>

        <button
          @click="addToCart"
          class="mt-8 w-full bg-indigo-600 text-white py-3 px-4 rounded-md hover:bg-indigo-700 font-medium"
        >
          Agregar al Carrito
        </button>
      </div>
    </div>
  </div>
</template>
