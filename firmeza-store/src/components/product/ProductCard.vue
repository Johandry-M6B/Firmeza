<script setup>
import { computed } from 'vue';
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

// Compute stock badge
const stockBadge = computed(() => {
  const stock = props.product.currentStock || 0;
  const minStock = props.product.minimumStock || 10;
  
  if (stock === 0) {
    return { text: 'Agotado', class: 'bg-red-500' };
  } else if (stock <= minStock) {
    return { text: 'Stock Bajo', class: 'bg-yellow-500' };
  } else if (stock > minStock * 2) {
    return { text: 'En Stock', class: 'bg-green-500' };
  }
  return null;
});

// Format price
const formattedPrice = computed(() => {
  const price = props.product.salePrice || 0;
  return new Intl.NumberFormat('es-CO', {
    style: 'currency',
    currency: 'COP',
    minimumFractionDigits: 0,
    maximumFractionDigits: 0,
  }).format(price);
});

// Format stock display
const stockDisplay = computed(() => {
  const stock = props.product.currentStock || 0;
  const unit = props.product.measurement?.abbreviation || 'Unid';
  return `Stock: ${stock} ${unit}`;
});
</script>

<template>
  <div class="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-xl transition-shadow duration-300">
    <!-- Image Container with Badge -->
    <div class="relative">
      <div class="aspect-w-1 aspect-h-1 w-full bg-gray-200">
        <div class="flex items-center justify-center h-48 bg-gradient-to-br from-gray-100 to-gray-200">
          <span class="text-gray-400 text-5xl">📦</span>
        </div>
      </div>
      
      <!-- Stock Badge -->
      <div v-if="stockBadge" class="absolute top-2 left-2">
        <span :class="[stockBadge.class, 'text-white text-xs font-bold px-3 py-1 rounded-full shadow-lg']">
          {{ stockBadge.text }}
        </span>
      </div>
    </div>

    <!-- Product Info -->
    <div class="p-4">
      <h3 class="text-lg font-bold text-gray-900 truncate mb-1">{{ product.name }}</h3>
      <p class="text-sm text-gray-500 line-clamp-2 mb-3" style="min-height: 2.5rem;">
        {{ product.description || 'Sin descripción disponible' }}
      </p>
      
      <!-- Stock Info -->
      <div class="flex items-center text-xs text-gray-600 mb-3">
        <svg class="h-4 w-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4"/>
        </svg>
        {{ stockDisplay }}
      </div>

      <!-- Price and Actions -->
      <div class="border-t pt-3">
        <div class="flex items-center justify-between mb-3">
          <div>
            <div class="text-2xl font-bold text-blue-600">{{ formattedPrice }}</div>
            <div class="text-xs text-gray-500">/ {{ product.measurement?.abbreviation || 'Unid' }}</div>
          </div>
        </div>

        <!-- Action Buttons -->
        <div class="flex gap-2">
          <RouterLink
            :to="`/products/${product.id}`"
            class="flex-1 px-3 py-2 bg-white border-2 border-blue-600 text-blue-600 text-sm font-medium rounded-md hover:bg-blue-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition-colors text-center"
          >
            Ver Detalles
          </RouterLink>
          <button
            @click="addToCart"
            :disabled="(product.currentStock || 0) === 0"
            :class="[
              'flex-1 px-3 py-2 text-white text-sm font-medium rounded-md focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition-colors',
              (product.currentStock || 0) === 0
                ? 'bg-gray-400 cursor-not-allowed'
                : 'bg-blue-600 hover:bg-blue-700'
            ]"
          >
            {{ (product.currentStock || 0) === 0 ? 'Agotado' : 'Agregar al Carrito' }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
