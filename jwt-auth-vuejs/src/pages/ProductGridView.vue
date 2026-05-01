<template>
    <div class="row justify-content-md-center">
        <div class="col-md-8"> 
            <div v-if="products.length > 0" class="row mb-3">
                <div class="col-12">
                    <div class="d-flex justify-content-between align-items-center">
                        <h5 class="text-muted mb-0">
                            Showing {{ products.length }} products
                        </h5>
                    </div>
                </div>
            </div>

            <div v-if="products.length > 0" class="row">
                <div v-for="product in products" class="col-lg-3 col-md-4 col-sm-6 mb-4" :key="product">
                    <div class="card h-100 shadow-sm product-card">
                        <div class="position-relative clickable-area">
                            <!-- Hover overlay -->
                            <div class="image-overlay">
                                <i class="fas fa-eye"></i>
                                <span>View Details</span>
                            </div>
                            <img :src="product.thumbnail"
                                 :alt="product.title"
                                 class="card-img-top product-image"
                                 style="height: 200px; object-fit: contain; padding: 10px; background-color: #f8f9fa;"
                                 >


                        </div>
                        <div class="card-body d-flex flex-column">
                            <div class="d-flex justify-content-between mb-2">
                                <span class="badge bg-light text-dark">
                                    {{ product.category }}
                                </span>
                                <small v-if="product.brand" class="text-muted">
                                    {{ product.brand }}
                                </small>
                            </div>

                            <!-- Title - Clickable -->
                            <h6 class="card-title clickable-title" [title]="product.title">
                                {{ product.title }}
                            </h6>

                            <!-- Description -->
                            <p class="card-text text-muted small flex-grow-1">
                                {{ product.description }}
                            </p>

                            <div class="mt-auto">
                                <div class="d-flex justify-content-between align-items-center">
                                    <div>
                                        <h5 class="text-primary mb-0">${{ product.price }}</h5>
                                    </div>
                                    <button type="button"
                                            class="btn btn-primary btn-sm"
                                            >
                                        <i class="fas fa-cart-plus me-1"></i>
                                        {{'addToCart'}}
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <span>{{errorMessage}}</span>
        </div>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import MainService from '../services/mainService';

    const products = ref([]);

    onMounted(async () => {
        try {
            const response = await MainService.getProducts();
            products.value = response.data;
        } catch (error) {
            console.error('Error fetching products:', error);
        }
    });
</script>