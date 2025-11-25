<script setup>
import { useRouter } from 'vue-router';
import { useCartStore } from '../stores/cartStore';
import { TrashIcon } from '@heroicons/vue/24/outline';

const router = useRouter();
const cartStore = useCartStore();

const goToCheckout = () => {
  router.push('/checkout');
};
</script>

<template>
  <div class="max-w-7xl mx-auto py-8 px-4 sm:px-6 lg:px-8">
    <h1 class="text-3xl font-bold text-gray-900 mb-8">Carrito de Compras</h1>

    <div v-if="cartStore.items.length === 0" class="text-center py-12">
      <p class="text-gray-500 text-lg">Tu carrito está vacío</p>
      <RouterLink
        to="/products"
        class="mt-4 inline-block text-indigo-600 hover:text-indigo-500 font-medium"
      >
        Ir a comprar →
      </RouterLink>
    </div>

    <div v-else class="grid grid-cols-1 lg:grid-cols-3 gap-8">
      <div class="lg:col-span-2">
        <div class="bg-white shadow overflow-hidden sm:rounded-md">
          <ul class="divide-y divide-gray-200">
            <li v-for="item in cartStore.items" :key="item.id" class="p-4">
              <div class="flex items-center justify-between">
                <div class="flex-1">
                  <h3 class="text-lg font-medium text-gray-900">{{ item.name }}</h3>
                  <p class="text-sm text-gray-500">${{ item.price.toFixed(2) }} c/u</p>
                </div>
                <div class="flex items-center space-x-4">
                  <div class="flex items-center space-x-2">
                    <button
                      @click="cartStore.updateQuantity(item.id, item.quantity - 1)"
                      class="px-2 py-1 border rounded"
                    >
                      -
                    </button>
                    <span class="px-4">{{ item.quantity }}</span>
                    <button
                      @click="cartStore.updateQuantity(item.id, item.quantity + 1)"
                      class="px-2 py-1 border rounded"
                    >
                      +
                    </button>
                  </div>
                  <span class="text-lg font-semibold text-gray-900 w-24 text-right">
                    ${{ (item.price * item.quantity).toFixed(2) }}
                  </span>
                  <button
                    @click="cartStore.removeItem(item.id)"
                    class="text-red-600 hover:text-red-800"
                  >
                    <TrashIcon class="h-5 w-5" />
                  </button>
                </div>
              </div>
            </li>
          </ul>
        </div>
      </div>

      <div class="lg:col-span-1">
        <div class="bg-white shadow rounded-lg p-6">
          <h2 class="text-lg font-medium text-gray-900 mb-4">Resumen del Pedido</h2>
          <div class="space-y-2">
            <div class="flex justify-between">
              <span class="text-gray-600">Subtotal</span>
              <span class="font-semibold">${{ cartStore.total.toFixed(2) }}</span>
            </div>
            <div class="flex justify-between text-lg font-bold border-t pt-2">
              <span>Total</span>
              <span>${{ cartStore.total.toFixed(2) }}</span>
            </div>
          </div>
          <button
            @click="goToCheckout"
            class="mt-6 w-full bg-indigo-600 text-white py-3 px-4 rounded-md hover:bg-indigo-700 font-medium"
          >
            Proceder al Pago
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
