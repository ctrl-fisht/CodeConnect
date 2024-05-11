<script>
import { Form, Field, ErrorMessage } from "vee-validate"
import * as yup from "yup";
export default {
  name: "EditGroupForm",
  components: {Form, Field, ErrorMessage},
  props: {
    groupInfo: {
      type: Object,
      required: true
    }
  },
  data() {
    let phoneRegex = /^(\+?\d{0,4})?\s?-?\s?(\(?\d{3}\)?)\s?-?\s?(\(?\d{3}\)?)\s?-?\s?(\(?\d{4}\)?)?$/;
    const schema = yup.object().shape({
      name: yup.string().required("Необходимо ввести название"),
      description: yup.string().required("Необходимо ввести описание").nullable(true),
      email: yup.string().email("Введите правильный email-адрес").nullable(true),
      phone: yup.string().test(
          'phone',
          'Введите правильный номер телефона',
          (value) => {
            if (value === null || value === '') {
              return true;
            }
            return phoneRegex.test(value);
          }
      ),
      telegramTag: yup.string().nullable(true),
    });
    return {
      schema,
      loading: false,
      successful: false,
    }
  },
  methods: {
    handleEdit(formData) {
      let payload = {
        "id": this.groupInfo.communityId,
        "name": this.groupInfo.name,
        "description": this.groupInfo.description,
        "email": this.groupInfo.email,
        "phone": this.groupInfo.phone,
        "telegramTag": this.groupInfo.telegramTag
      }

      if (formData.description !== payload.nescription)
        payload.description = formData.description;

      if (formData.name !== payload.name)
        payload.name = formData.name;

      if (formData.email !== payload.email)
        payload.email = formData.email;

      if (formData.phone !== payload.phone)
        payload.phone = formData.phone;

      if (formData.telegram !== payload.telegramTag)
        payload.telegramTag = formData.telegram;

      this.$store.dispatch("user/updateGroup", payload).then(
          data => {
            this.$emit("updateEvent", "");
          },
          error => {
            this.$notify({
              group: "error",
              title: "Ошибка",
              text: error.message
            }, 4000)
          }
      )
    }
  }
}
</script>

<template>
  <Form name="add" @submit="handleEdit" :validation-schema="schema">
    <div v-if="!successful">
      <div class="form-group ml-2 flex flex-col">
        <label for="name" class="text-sm">Название</label>
        <Field class="h-8 pl-2 bg-white shadow" name="name" type="text" :value="groupInfo.name"/>
        <ErrorMessage name="name" class="text-red-500"/>
      </div>
      <div class="form-group  ml-2 mt-4 flex flex-col">
        <label for="description" class="text-sm">Описание</label>
        <Field as="textarea" class="h-36 pl-2 bg-white shadow" name="description" type="text" :value="groupInfo.description"/>
        <ErrorMessage name="description" class="text-red-500" />
      </div>
      <div class="form-group ml-2 flex flex-col">
        <label for="email" class="text-sm">Email</label>
        <Field class="h-8 pl-2 bg-white shadow" name="email" type="email" :value="groupInfo.email"/>
        <ErrorMessage name="email" class="text-red-500"/>
      </div>
      <div class="form-group ml-2 flex flex-col">
        <label for="phone" class="text-sm">Телефон</label>
        <Field class="h-8 pl-2 bg-white shadow" name="phone" type="phone" :value="groupInfo.phone"/>
        <ErrorMessage name="phone" class="text-red-500"/>
      </div>
      <div class="form-group ml-2 flex flex-col">
        <label for="telegram" class="text-sm">Телеграм</label>
        <Field class="h-8 pl-2 bg-white shadow" name="telegram" type="text" :value="groupInfo.telegramTag"/>
        <ErrorMessage name="telegram" class="text-red-500"/>
      </div>
      <div v-show:="!loading"></div>
      <div role="status" v-show="loading">
        <svg aria-hidden="true" class="w-8 h-8 text-gray-200 animate-spin dark:text-gray-600 fill-blue-600" viewBox="0 0 100 101" fill="none" xmlns="http://www.w3.org/2000/svg">
          <path d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z" fill="currentColor"/>
          <path d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z" fill="currentFill"/>
        </svg>
        <span class="sr-only">Loading...</span>
      </div>
      <div v-if="!successful" class="flex justify-between">
        <div></div>
        <div class="rounded cursor-pointer bg-amber-500 mt-4">
          <button class="px-12 py-2">Изменить</button>
        </div>
        <div></div>
      </div>
    </div>
  </Form>
</template>

<style scoped>

</style>