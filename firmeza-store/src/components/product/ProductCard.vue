<script setup>
import { RouterLink } from 'vue-router';
import { useCartStore } from '../../stores/cartStore';

const props = defineProps({
  product: {
    type: Object,
    required: true,
  },
});

const cartStore = useCartStore();

const addToCart = () => {
  cartStore.addItem(props.product);
};
</script>

<template>
  <div class="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow">
    <div class="aspect-w-1 aspect-h-1 w-full bg-gray-200">
      <div class="flex items-center justify-center h-48 bg-gray-100">
        <span class="text-gray-400 text-4xl">📦</span>
      </div>
    </div>
    <div class="p-4">
      <h3 class="text-lg font-semibold text-gray-900 truncate">{{ product.name }}</h3>
      <p class="mt-1 text-sm text-gray-500 line-clamp-2">{{ product.description }}</p>
      <div class="mt-4 flex items-center justify-between">
        <span class="text-2xl font-bold text-indigo-600">${{ product.salePrice?.toFixed(2) }}</span>
        <button
          @click="addToCart"
          class="px-4 py-2 bg-indigo-600 text-white text-sm font-medium rounded-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
        >
          Agregar
        </button>
      </div>
      <RouterLink
        :to="`/products/${product.id}`"
        class="mt-2 block text-sm text-indigo-600 hover:text-indigo-500"
      >
        Ver detalles →
      </RouterLink>
    </div>
  </div>
</template>
