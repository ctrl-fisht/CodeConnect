<template>
  <header class="bg-gray-50 drop-shadow-sm h-16 shadow">
    <div class="container mx-auto flex justify-between items-center">
        <span class="flex pt-3 pb-5 items-end">
            <img src="@/static/logo.svg" alt="logo"/>

            <router-link to="/events" class="logo font-semibold text-orange-500">CodeConnect</router-link>
        </span>
      <div class="flex justify-between  items-center">
        <nav>
          <ul class="flex">
            <li v-if="this.currentUser">
              <DropdownMember v-model:isOpen="memberDropdownVisible"/>
            </li>
            <li v-if="this.currentUser" class="mx-5">
              <DropdownOwner v-model:isOpen="ownerDropdownVisible"/>
            </li>
            <li class="mx-5 hover:text-orange-400"><router-link to="/events">Мероприятия</router-link></li>
          </ul>
        </nav>
        <DropdownUser v-if="this.currentUser" v-model:isOpen="userDropdownVisible"/>
        <my-button class="bg-amber-400 hover:bg-amber-300 cursor-pointer mr-4 py-1 px-2 rounded" @click="loginDialogRequest" v-if="!this.currentUser">Войти</my-button>
        <my-button class="hover:text-orange-400 cursor-pointer" @click="registerDialogRequest" v-if="!this.currentUser">Регистрация</my-button>

      </div>
    </div>
  </header>
  <Dialog v-model:visible="loginDialogVisible" modal header="Вход в аккаунт" :style="{ width: '25rem' }">
    <login-form
        @logined="loginedAlready"
    />
  </Dialog>
  <Dialog v-model:visible="registerDialogVisible" modal header="Регистрация" :style="{ width: '25rem' }">
    <register-form
        @registered="registeredAlready"
    />
  </Dialog>
</template>

<script>
import LoginForm from "@/components/LoginForm.vue"
import RegisterForm from "@/components/RegisterForm.vue"
import DropdownMember from "@/components/DropdownMember.vue"
import DropdownOwner from "@/components/DropdownOwner.vue"
import DropdownUser from "@/components/DropdownUser.vue"
export default {
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
  },
  components: { LoginForm, RegisterForm, DropdownMember, DropdownOwner, DropdownUser },
  mounted() {
    this.checkTokenExpiration();
    // Запуск функции каждые несколько секунд
    setInterval(this.checkTokenExpiration, 5000);
  },
  data() {
    return{
      loginDialogVisible: false,
      registerDialogVisible: false,
      memberDropdownVisible: false,
      ownerDropdownVisible: false,
      userDropdownVisible: false,
    }
  },
  methods: {
    checkTokenExpiration() {
      const user = JSON.parse(localStorage.getItem('user'));
      if (user && user.expires) {
        const currentTime = Date.now();
        if (currentTime >= user.expires) {
          localStorage.removeItem('user');
        }
      }
    },
    loginDialogRequest() {
      this.loginDialogVisible = true;
    },
    registerDialogRequest() {
      this.registerDialogVisible = true;
      this.loginDialogVisible = false;
    },
    loginedAlready() {
      this.loginDialogVisible = false;
    },
    registeredAlready() {
      this.registerDialogVisible = false;
    }
  }

}

</script>

<style scoped>

</style>