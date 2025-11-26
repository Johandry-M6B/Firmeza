<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { authService } from '../../services/authService';

const router = useRouter();

const formData = ref({
  fullName: '',
  email: '',
  phoneNumber: '',
  documentNumber: '',
  city: '',
  country: ''
});

const error = ref('');
const loading = ref(false);
const successMessage = ref('');

const handleRegister = async () => {
  error.value = '';
  successMessage.value = '';
  loading.value = true;

  try {
    await authService.register(formData.value);
    successMessage.value = '¡Registro exitoso! Redirigiendo al inicio de sesión...';
    
    // Redirect to login after 2 seconds
    setTimeout(() => {
      router.push('/login');
    }, 2000);
  } catch (err) {
    if (err.response?.data?.errors) {
      // FluentValidation errors
      const validationErrors = err.response.data.errors;
      error.value = Object.values(validationErrors).flat().join(', ');
    } else if (err.response?.data?.message) {
      error.value = err.response.data.message;
    } else {
      error.value = 'Error al registrar. Por favor, intenta nuevamente.';
    }
  } finally {
    loading.value = false;
  }
};
</script>

<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md w-full space-y-8">
      <div>
        <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
          Crear Cuenta
        </h2>
        <p class="mt-2 text-center text-sm text-gray-600">
          Regístrate para comenzar a comprar
        </p>
      </div>
      
      <form class="mt-8 space-y-6" @submit.prevent="handleRegister">
        <div class="rounded-md shadow-sm space-y-4">
          <div>
            <label for="fullName" class="block text-sm font-medium text-gray-700">
              Nombre Completo *
            </label>
            <input
              id="fullName"
              v-model="formData.fullName"
              type="text"
              required
              class="mt-1 appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="Juan Pérez"
            />
          </div>

          <div>
            <label for="email" class="block text-sm font-medium text-gray-700">
              Email *
            </label>
            <input
              id="email"
              v-model="formData.email"
              type="email"
              required
              class="mt-1 appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="juan@ejemplo.com"
            />
          </div>

          <div>
            <label for="documentNumber" class="block text-sm font-medium text-gray-700">
              Número de Documento *
            </label>
            <input
              id="documentNumber"
              v-model="formData.documentNumber"
              type="text"
              required
              class="mt-1 appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="12345678"
            />
          </div>

          <div>
            <label for="phoneNumber" class="block text-sm font-medium text-gray-700">
              Teléfono
            </label>
            <input
              id="phoneNumber"
              v-model="formData.phoneNumber"
              type="tel"
              class="mt-1 appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="+57 300 123 4567"
            />
          </div>

          <div>
            <label for="city" class="block text-sm font-medium text-gray-700">
              Ciudad
            </label>
            <input
              id="city"
              v-model="formData.city"
              type="text"
              class="mt-1 appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="Bogotá"
            />
          </div>

          <div>
            <label for="country" class="block text-sm font-medium text-gray-700">
              País
            </label>
            <input
              id="country"
              v-model="formData.country"
              type="text"
              class="mt-1 appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="Colombia"
            />
          </div>
        </div>

        <div v-if="error" class="text-red-600 text-sm text-center bg-red-50 p-3 rounded-md">
          {{ error }}
        </div>

        <div v-if="successMessage" class="text-green-600 text-sm text-center bg-green-50 p-3 rounded-md">
          {{ successMessage }}
        </div>

        <div>
          <button
            type="submit"
            :disabled="loading"
            class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {{ loading ? 'Registrando...' : 'Registrarse' }}
          </button>
        </div>

        <div class="text-center">
          <router-link to="/login" class="text-sm text-indigo-600 hover:text-indigo-500">
            ¿Ya tienes cuenta? Inicia sesión aquí
          </router-link>
        </div>
      </form>
    </div>
  </div>
</template>
