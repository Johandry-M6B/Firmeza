import { defineStore } from 'pinia';

export const useCartStore = defineStore('cart', {
    state: () => ({
        items: JSON.parse(localStorage.getItem('cart')) || [],
    }),

    getters: {
        itemCount: (state) => state.items.reduce((total, item) => total + item.quantity, 0),
        total: (state) => state.items.reduce((total, item) => total + item.price * item.quantity, 0),
    },

    actions: {
        addItem(product, quantity = 1) {
            const existingItem = this.items.find((item) => item.id === product.id);

            if (existingItem) {
                existingItem.quantity += quantity;
            } else {
                this.items.push({
                    id: product.id,
                    name: product.name,
                    price: product.salePrice,
                    quantity,
                });
            }

            this.saveToLocalStorage();
        },

        removeItem(productId) {
            this.items = this.items.filter((item) => item.id !== productId);
            this.saveToLocalStorage();
        },

        updateQuantity(productId, quantity) {
            const item = this.items.find((item) => item.id === productId);
            if (item) {
                item.quantity = quantity;
                if (item.quantity <= 0) {
                    this.removeItem(productId);
                } else {
                    this.saveToLocalStorage();
                }
            }
        },

        clear() {
            this.items = [];
            this.saveToLocalStorage();
        },

        saveToLocalStorage() {
            localStorage.setItem('cart', JSON.stringify(this.items));
        },
    },
});
