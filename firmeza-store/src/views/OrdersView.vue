<script setup>
import { ref, onMounted } from 'vue';
import { salesService } from '../services/salesService';

const orders = ref([]);
const loading = ref(true);

onMounted(async () => {
  try {
    const data = await salesService.getMyOrders();
    orders.value = data;
  } catch (error) {
    console.error('Error loading orders:', error);
  } finally {
    loading.value = false;
  }
});
</script>

<template>
  <div class="max-w-7xl mx-auto py-8 px-4 sm:px-6 lg:px-8">
    <h1 class="text-3xl font-bold text-gray-900 mb-8">Mis Órdenes</h1>

    <div v-if="loading" class="text-center py-12">
      <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
    </div>

    <div v-else-if="orders.length === 0" class="text-center py-12">
      <p class="text-gray-500 text-lg">No tienes órdenes aún</p>
      <RouterLink
        to="/products"
        class="mt-4 inline-block text-indigo-600 hover:text-indigo-500 font-medium"
      >
        Ir a comprar →
      </RouterLink>
    </div>

    <div v-else class="space-y-4">
      <div
        v-for="order in orders"
        :key="order.id"
        class="bg-white shadow rounded-lg p-6"
      >
        <div class="flex justify-between items-start mb-4">
          <div>
            <h3 class="text-lg font-semibold text-gray-900">Orden #{{ order.id }}</h3>
            <p class="text-sm text-gray-500">{{ new Date(order.saleDate).toLocaleDateString() }}</p>
          </div>
          <span class="px-3 py-1 text-sm font-semibold rounded-full bg-green-100 text-green-800">
            {{ order.status }}
          </span>
        </div>
        <div class="text-right">
          <span class="text-2xl font-bold text-indigo-600">${{ order.total?.toFixed(2) }}</span>
        </div>
      </div>
    </div>
  </div>
</template>
