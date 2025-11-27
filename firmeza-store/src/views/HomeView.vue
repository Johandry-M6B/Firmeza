<script setup>
import { ref, onMounted } from 'vue';
import { RouterLink } from 'vue-router';
import { productsService } from '../services/productsService';
import ProductCard from '../components/product/ProductCard.vue';
import { BuildingStorefrontIcon } from '@heroicons/vue/24/outline';

const products = ref([]);
const loading = ref(true);
const selectedCategory = ref('all');

const categories = [
  { id: 'all', name: 'Todas las categorías', icon: '📦' },
  { id: 'alimentos', name: 'Alimentos', icon: '🍎' },
  { id: 'bebidas', name: 'Bebidas', icon: '🥤' },
  { id: 'electronica', name: 'Electrónica', icon: '💻' },
  { id: 'ferreteria', name: 'Ferretería', icon: '🔧' },
  { id: 'limpieza', name: 'Limpieza', icon: '🧹' },
];

onMounted(async () => {
  try {
    const data = await productsService.getAll();
    products.value = data;
  } catch (error) {
    if (error.response?.status === 401) {
      // API requires authentication, show empty products list
      console.warn('Products require authentication. Showing empty list.');
      products.value = [];
    } else {
      console.error('Error loading products:', error);
      products.value = [];
    }
  } finally {
    loading.value = false;
  }
});

const filteredProducts = ref([]);
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Hero Section -->
    <div class="bg-gradient-to-r from-blue-600 to-blue-700 text-white">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
        <div class="flex items-center space-x-4">
          <BuildingStorefrontIcon class="h-16 w-16" />
          <div>
            <h1 class="text-4xl font-bold">Tienda de Materiales</h1>
            <p class="text-xl text-blue-100 mt-2">Encuentra todo lo que necesitas para tu construcción</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Main Content -->
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div class="flex gap-6">
        <!-- Sidebar Categories -->
        <aside class="w-64 flex-shrink-0">
          <div class="bg-white rounded-lg shadow-md overflow-hidden">
            <div class="bg-gray-800 text-white px-4 py-3 flex items-center space-x-2">
              <svg class="h-5 w-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16"/>
              </svg>
              <span class="font-semibold">Categorías</span>
            </div>
            <ul class="divide-y divide-gray-200">
              <li
                v-for="category in categories"
                :key="category.id"
                @click="selectedCategory = category.id"
                :class="[
                  'px-4 py-3 cursor-pointer transition-colors',
                  selectedCategory === category.id
                    ? 'bg-blue-50 text-blue-700 font-medium'
                    : 'hover:bg-gray-50 text-gray-700'
                ]"
              >
                <span class="mr-2">{{ category.icon }}</span>
                {{ category.name }}
              </li>
            </ul>
          </div>

          <!-- Contact Info -->
          <div class="mt-6 bg-white rounded-lg shadow-md p-4">
            <h3 class="font-semibold text-gray-800 mb-3 flex items-center">
              <svg class="h-5 w-5 mr-2 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/>
              </svg>
              Firmeza
            </h3>
            <p class="text-sm text-gray-600 mb-2">Materiales de Construcción de Calidad</p>
            <div class="space-y-2 text-sm text-gray-600">
              <div class="flex items-center">
                <span class="mr-2">📞</span>
                <span>(055) 300-1234</span>
              </div>
              <div class="flex items-center">
                <span class="mr-2">✉️</span>
                <span>ventas@firmeza.com</span>
              </div>
            </div>
          </div>

          <!-- Schedule -->
          <div class="mt-6 bg-white rounded-lg shadow-md p-4">
            <h3 class="font-semibold text-gray-800 mb-3">Horario</h3>
            <div class="space-y-1 text-sm text-gray-600">
              <p>Lunes a Viernes: 8:00 AM - 6:00 PM</p>
              <p>Sábados: 8:00 AM - 2:00 PM</p>
            </div>
          </div>
        </aside>

        <!-- Products Grid -->
        <main class="flex-1">
          <!-- Info Message -->
          <div v-if="products.length === 0 && !loading" class="bg-blue-50 border border-blue-200 rounded-lg p-4 mb-6">
            <div class="flex items-center">
              <svg class="h-5 w-5 text-blue-600 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/>
              </svg>
              <span class="text-blue-800">No se encontraron productos. Intenta con otra búsqueda.</span>
            </div>
          </div>

          <!-- Loading State -->
          <div v-if="loading" class="flex justify-center items-center py-20">
            <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
          </div>

          <!-- Products -->
          <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <ProductCard v-for="product in products" :key="product.id" :product="product" />
          </div>
        </main>
      </div>
    </div>

    <!-- Footer -->
    <footer class="bg-gray-800 text-white mt-12">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
          <div>
            <h3 class="text-lg font-semibold mb-4 flex items-center">
              <svg class="h-6 w-6 mr-2" fill="currentColor" viewBox="0 0 24 24">
                <path d="M12 2L2 7v10c0 5.55 3.84 10.74 9 12 5.16-1.26 9-6.45 9-12V7l-10-5z"/>
              </svg>
              Firmeza
            </h3>
            <p class="text-gray-400 text-sm">Materiales de Construcción de Calidad</p>
          </div>
          <div>
            <h3 class="text-lg font-semibold mb-4">Contacto</h3>
            <div class="space-y-2 text-sm text-gray-400">
              <p>📞 (055) 300-1234</p>
              <p>✉️ ventas@firmeza.com</p>
            </div>
          </div>
          <div>
            <h3 class="text-lg font-semibold mb-4">Horario</h3>
            <div class="space-y-1 text-sm text-gray-400">
              <p>Lunes a Viernes: 8:00 AM - 6:00 PM</p>
              <p>Sábados: 8:00 AM - 2:00 PM</p>
            </div>
          </div>
        </div>
        <div class="border-t border-gray-700 mt-8 pt-6 text-center text-sm text-gray-400">
          © 2025 - Firmeza. Todos los derechos reservados.
        </div>
      </div>
    </footer>
  </div>
</template>
