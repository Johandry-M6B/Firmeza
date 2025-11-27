<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useCartStore } from '../stores/cartStore';
import { salesService } from '../services/salesService';

const router = useRouter();
const cartStore = useCartStore();

const loading = ref(false);
const error = ref('');

const handleCheckout = async () => {
  loading.value = true;
  error.value = '';

  try {
    // Create sale with cart items
    const saleData = {
      items: cartStore.items.map(item => ({
        productId: item.id,
        quantity: item.quantity,
        unitPrice: item.price,
      })),
    };

    await salesService.create(saleData);
    cartStore.clear();
    router.push('/orders');
  } catch (err) {
    error.value = 'Error al procesar la compra. Por favor intenta de nuevo.';
    console.error('Checkout error:', err);
  } finally {
    loading.value = false;
  }
};
</script>

<template>
  <div class="max-w-3xl mx-auto py-8 px-4 sm:px-6 lg:px-8">
    <h1 class="text-3xl font-bold text-gray-900 mb-8">Finalizar Compra</h1>

    <div class="bg-white shadow rounded-lg p-6 mb-6">
      <h2 class="text-lg font-medium text-gray-900 mb-4">Resumen de tu Pedido</h2>
      <ul class="divide-y divide-gray-200">
        <li v-for="item in cartStore.items" :key="item.id" class="py-3 flex justify-between">
          <div>
            <span class="font-medium">{{ item.name }}</span>
            <span class="text-gray-500 ml-2">x{{ item.quantity }}</span>
          </div>
          <span class="font-semibold">${{ (item.price * item.quantity).toFixed(2) }}</span>
        </li>
      </ul>
      <div class="mt-4 pt-4 border-t flex justify-between text-lg font-bold">
        <span>Total</span>
        <span>${{ cartStore.total.toFixed(2) }}</span>
      </div>
    </div>

    <div v-if="error" class="bg-red-50 border border-red-200 text-red-600 px-4 py-3 rounded mb-4">
      {{ error }}
    </div>

    <div class="flex space-x-4">
      <button
        @click="router.push('/cart')"
        class="flex-1 bg-gray-200 text-gray-700 py-3 px-4 rounded-md hover:bg-gray-300 font-medium"
      >
        Volver al Carrito
      </button>
      <button
        @click="handleCheckout"
        :disabled="loading"
        class="flex-1 bg-indigo-600 text-white py-3 px-4 rounded-md hover:bg-indigo-700 font-medium disabled:opacity-50"
      >
        {{ loading ? 'Procesando...' : 'Confirmar Compra' }}
      </button>
    </div>
  </div>
</template>
