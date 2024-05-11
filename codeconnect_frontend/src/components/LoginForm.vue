<template>
  <div class="">
    <div class="flex flex-col justify-around content-center">
      <Form @submit="handleLogin" :validation-schema="schema">
        <div class="">
          <div class="form-group ml-2 flex flex-col">
            <label for="username" class="text-sm">Логин/Email</label>
            <Field name="username" type="text" class="h-8 pl-2 bg-white shadow" />
            <ErrorMessage name="username" class="text-red-500" />
          </div>
          <div class="form-group ml-2 mt-4 flex flex-col">
            <label for="password" class="text-sm">Пароль</label>
            <Field name="password" type="password" class="h-8 pl-2 bg-white shadow" />
            <ErrorMessage name="password" class="text-red-500" />
          </div>


        </div>
        <div class="form-group justify-self-center ml-2">
          <div v-if="messages.length > 0" class="text-red-500 mt-5" role="alert">
            <div v-for="message in messages">
              {{ message }}
            </div>
          </div>
        </div>
        <div class="flex mt-4  justify-between">
          <div v-show:="!loading"></div>
          <div role="status" v-show="loading">
            <svg aria-hidden="true" class="w-8 h-8 text-gray-200 animate-spin dark:text-gray-600 fill-blue-600" viewBox="0 0 100 101" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z" fill="currentColor"/>
              <path d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z" fill="currentFill"/>
            </svg>
            <span class="sr-only">Loading...</span>
          </div>
          <div class="rounded cursor-pointer bg-amber-300">
            <button :disabled="loading" class="px-12 py-2">
              <span>Login</span>
            </button>
          </div>

          <div></div>
        </div>



      </Form>
    </div>
  </div>
</template>

<script>
import { Form, Field, ErrorMessage } from "vee-validate"
import * as yup from "yup";
export default {
  name: "LoginForm",
  components: {
    Form, Field, ErrorMessage
  },
  data() {
    const schema = yup.object().shape({
      username: yup.string().required("Необходимо ввести логин/email"),
      password: yup.string().required("Необходимо ввести пароль"),
    });

    return {
      loading: false,
      messages: [],
      schema,
      loginFormData: {
        login: '',
        password: ''
      }
    }
  },
  computed: {
    loggedIn() {
      return this.$store.state.auth.status.loggedIn;
    }
  },
  created() {
    if (this.loggedIn){
      this.$emit('logined', "");
      this.$router.push("/events");
    }
  },
  methods: {
    handleLogin(user) {
      this.loading = true;

      this.$store.dispatch("auth/login", user).then(
          () => {
            this.$emit('logined', "");
            this.messages = []
            this.$router.push("/events");
            this.$notify({
              group: "notif",
              title: "Успешно",
              text: "Вы вошли в аккаунт"
            }, 4000)
          },
          (error) => {
            this.messages = [];
            this.loading = false;

            if (error.response?.data?.errors?.Password)
              this.messages.push(error.response.data.errors.Password.join());

            if (error.response?.data?.errors?.Username)
              this.messages.push(error.response.data.errors.Username.join());

            if (this.messages.length === 0)
                this.messages.push("Неверный логин или пароль");
          }
      );
    },
    registerRequest() {
      this.$emit('registerFormRequest');
    }

  }
}
</script>

<style scoped>

</style>