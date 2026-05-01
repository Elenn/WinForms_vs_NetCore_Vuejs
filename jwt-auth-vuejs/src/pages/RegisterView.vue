<template>
    <div class="row justify-content-md-center">
        <div class="col-md-3">
            <h1 class="h3 mb-3 fw-normal">Please Register</h1>

            <input v-model="inputUserName" type="email" class="form-control" placeholder="Email" required>
            <input v-model="inputPassword" type="password" class="form-control" placeholder="Password" required>
            <input v-model="inputPassword" type="password" class="form-control" placeholder="Password" required>

            <button class="w-100 btn btn-lg btn-primary" @click="register">Register</button> 
        </div>
    </div>
</template>

<script setup>
    import { ref } from 'vue';
    import { useRouter } from 'vue-router';
    import { useStore } from 'vuex';
    import MainService from '../services/mainService';

    const inputUserName = ref('');
    const inputPassword = ref('');
    const errorMessage = ref('');
    const loading = ref(false);

    const router = useRouter();
    const store = useStore();

    const register = async () => {
        errorMessage.value = '';
        loading.value = true;

        try {
            const response = await MainService.register({
                username: inputUserName.value,
                password: inputPassword.value
            });

            // Save auth state and token via Vuex actions
            store.dispatch('setAuth', true);
            store.dispatch('setToken', response.data.jwt);
            localStorage.setItem('token', response.data.jwt);

            // Navigate to posts (keeps original behavior)
            router.push('productgrid');
        } catch (err) {
            console.error('Login error:', err);
            store.dispatch('setAuth', false);

            // Prefer server message when available
            const serverMsg = err?.response?.data?.message || err?.response?.data || null;
            errorMessage.value = serverMsg || 'Register failed.';
        } finally {
            loading.value = false;
        }
    };
</script>