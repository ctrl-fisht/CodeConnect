<script>
import { Form, Field, ErrorMessage } from "vee-validate"

import * as yup from "yup";
export default {
  name: "EditEventForm",
  props: {
    eventData: {
      type: Object,
      required: true
    },
    groupsList: {
      type: Object,
      required: true
    },
    cityList: {
      type: Object,
      required: true
    },
    categoryList: {
      type: Object,
      required: true
    },
    tagsList: {
      type: Object,
      required: true
    }
  },
  computed: {
    currentData() {
      return {
        title: this.eventData.title,
        description: this.eventData.description,
        city: this.eventData.city.name,
        dateLocal: this.eventData.dateLocal,
        duration: this.eventData.duration,
        community: this.eventData.community.communityId,
        online: this.eventData.online,
        ticketPrice: this.eventData.ticketPrice,
        hasStream: this.eventData.hasStream
      }
    },
    currentDate() {
      let date = new Date(this.eventData.dateTime);
      let month = date.getMonth() + 1;
      if (month < 10)
        month = `0${month}`;
      return `${date.getFullYear()}-${month}-${date.getDate()}`;
    },
    tagsOptions() {
      let tags = []
      this.tagsList.forEach((tl) => {
        tags.push(tl.tagId);
      });
      return tags;
    }
  },
  components: {Form, Field, ErrorMessage},
  data() {
    return {
      loading: false,
      successful: false,
      streamCheckbox: false,
      onlineCheckbox: false,
      selectedTags: [],
      selectedCategories: []
    }
  },
  methods: {
    handleEdit(formData) {
      let payload = {
        "activityId": this.eventData.activityId,
        "title": this.eventData.title,
        "dateLocal": this.eventData.dateLocal,
        "timeLocal": this.eventData.timeLocal,
        "cityId": this.eventData.city.cityId,
        "communityId": this.eventData.community.communityId,
        "description": this.eventData.description,
        "address": this.eventData.address,
        "durationMinutes": this.eventData.durationMinutes,
        "hasStream": this.eventData.hasStream,
        "websiteUrl": this.eventData.websiteUrl,
        "ticketPrice": this.eventData.ticketPrice,
        "categoriesIds": [],
        "tagsIds": []
      }
      this.eventData.activityCategories.forEach(ac => payload.categoriesIds.push(ac.category.categoryId));
      this.eventData.activityTags.forEach(at => payload.tagsIds.push(at.tag.tagId));

      // title
      if (formData.title !== payload.title)
        payload.title = formData.title;
      // dateLocal
      if (formData.dateLocal !== payload.dateLocal)
        payload.dateLocal = formData.dateLocal;
      // timeLocal
      if (formData.timeLocal !== payload.timeLocal)
        payload.timeLocal = formData.timeLocal;
      // cityId
      if (formData.cityId !== undefined)
        payload.cityId = formData.cityId;
      // communityId
      if (formData.communityId !== undefined)
        payload.communityId = formData.communityId;
      // description
      if (formData.description !== payload.Description)
        payload.description = formData.description;
      // address
      if (formData.address !== payload.address)
        payload.address = formData.address;
      // durationMinutes
      if (formData.durationMinutes !== payload.durationMinutes)
        payload.durationMinutes = formData.durationMinutes;
      // hasStream
      if (formData.hasStream !== payload.hasStream)
        payload.hasStream = formData.hasStream;
      // websiteUrl
      if (formData.websiteUrl !== payload.websiteUrl && formData.websiteUrl !== "" && formData.websiteUrl !== undefined)
        payload.websiteUrl = formData.websiteUrl;
      // ticketPrice
      if (formData.ticketPrice !== payload.ticketPrice)
        payload.ticketPrice = formData.ticketPrice;


      payload.tagsIds = [];
      this.selectedTags.forEach((st) => {
        payload.tagsIds.push(st.tagId);
      });

      if (this.selectedTags.length === 0)
        payload.tagsIds = [];

      payload.categoriesIds = [];
      this.selectedCategories.forEach((sc) => {
        payload.categoriesIds.push(sc.categoryId);
      });

      if (this.selectedCategories.length === 0)
        payload.categoriesIds = [];

      this.$store.dispatch("user/updateEvent", payload).then(
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
    },
    fetchCheckboxes() {
      this.streamCheckbox = this.eventData.hasStream;
      this.onlineCheckbox = this.eventData.online;
    },
    fetchSelectedTags(){

      this.eventData.activityTags.forEach((at) => {
        let mappedTag = this.tagsList.find((t) => t.tagId === at.tag.tagId);
        this.selectedTags.push(mappedTag);
      });
    },
    fetchSelectedCategories(){
      this.eventData.activityCategories.forEach((ac) => {
        let mappedCategory = this.categoryList.find((c) => c.categoryId === ac.category.categoryId);
        this.selectedCategories.push(mappedCategory);
      });
    }
  },
  mounted() {
    this.fetchCheckboxes();
    this.fetchSelectedTags();
    this.fetchSelectedCategories();
  }
}
</script>

<template>
  <div class="flex flex-col justify-center">
    <Form name="edit" @submit="handleEdit">
      <div v-if="!successful">
        <div class="form-group ml-2 flex flex-col">
          <label for="title" class="text-sm">Название</label>
          <Field class="h-8 pl-2 bg-white shadow" name="title" type="text" :value="eventData.title"/>
          <ErrorMessage name="title" class="text-red-500"/>
        </div>
        <div class="form-group  ml-2 mt-4 flex flex-col">
          <label for="dateLocal" class="text-sm">Дата</label>
          <Field class="h-8 pl-2 bg-white shadow" :value="eventData.dateLocal"  name="dateLocal" type="date"/>
          <ErrorMessage name="dateLocal" class="text-red-500" />
        </div>
        <div class="form-group  ml-2 mt-4 flex flex-col">
          <label for="timeLocal" class="text-sm">Время</label>
          <Field class="h-8 pl-2 bg-white shadow" :value="eventData.timeLocal"  name="timeLocal" type="time" step="00:00:00"/>
          <ErrorMessage name="timeLocal" class="text-red-500" />
        </div>
        <div class="form-group  ml-2 mt-4 flex flex-col">
          <label for="communityId" class="text-sm">Сообщество</label>
          <Field as="select" class="h-8 pl-2 bg-white shadow" name="communityId" >
            <option value="" disabled>{{eventData.community.name}}</option>
            <option :value="community.communityId"  v-for="community in groupsList" > {{community.name}} </option>
          </Field>
          <ErrorMessage name="communityId" class="text-red-500" />
        </div>
        <div class="form-group  ml-2 mt-4 flex flex-col">
          <label for="ticketPrice" class="text-sm">Цена за вход</label>
          <Field class="flex flex-start h-8 pl-2 bg-white shadow" type="text" name="ticketPrice" :value="eventData.ticketPrice"/>
          <ErrorMessage name="ticketPrice" class="text-red-500" />
        </div>
        <div class="form-group  ml-2 mt-4 flex flex-col">
          <label for="cityId" class="text-sm">Город</label>
          <Field as="select" class="h-8 pl-2  bg-white shadow" name="cityId" >
            <option value="" disabled>{{eventData.city.name}}</option>
            <option :value="city.cityId"  v-for="city in cityList" > {{city.name}} </option>
          </Field>
          <ErrorMessage name="cityId" class="text-red-500" />
        </div>
        <div class="form-group ml-2 mt-4 flex flex-col">
          <label for="address" class="text-sm">Адрес</label>
          <Field class="h-8 pl-2 bg-white shadow" name="address" type="text" :value="eventData.address"/>
          <ErrorMessage name="address" class="text-red-500"/>
        </div>
        <div class="form-group  ml-2 mt-4 flex flex-col">
          <label for="durationMinutes" class="text-sm">Длительность (минут)</label>
          <Field class="h-8 pl-2 bg-white shadow" type="text" name="durationMinutes" :value="eventData.durationMinutes"/>
          <ErrorMessage name="durationMinutes" class="text-red-500" />
        </div>
        <div class="form-group ml-2 mt-4 flex flex-col">
          <label for="websiteUrl" class="text-sm">Ссылка на веб-сайт</label>
          <Field class="h-8 pl-2 bg-white shadow" name="websiteUrl" type="url" :value="eventData.websiteURL" pattern="https://.*"/>
          <ErrorMessage name="websiteUrl" class="text-red-500"/>
        </div>
        <div class="form-group ml-2 mt-4 flex flex-col">
          <label for="tags">Тэги</label>
          <MultiSelect name="tags" v-model="selectedTags" display="chip" :options="tagsList" optionLabel="title" placeholder="Выберите тэги"
                       :maxSelectedLabels="5" class="w-full bg-white shadow" />
        </div>
        <div class="form-group ml-2 mt-4 flex flex-col">
          <label for="categories">Категории</label>
          <MultiSelect name="categories" v-model="selectedCategories" display="chip" :options="categoryList" optionLabel="title" placeholder="Выберите тэги"
                       :maxSelectedLabels="3" class="w-full  bg-white shadow" />
        </div>
        <div class="form-group  ml-2 mt-4 flex flex-col">
          <label for="hasStream" class="text-sm">Стрим</label>
          <Field class="flex flex-start h-8 pl-2  bg-white shadow" type="checkbox" v-model="streamCheckbox" :value="true" :unchecked-value="false" name="hasStream"/>
          <ErrorMessage name="hasStream" class="text-red-500" />
        </div>
<!--        <div class="form-group  ml-2 mt-4 flex flex-col">-->
<!--          <label for="streamlink" class="text-sm">Стрим-ссылка</label>-->
<!--          <Field class="flex flex-start h-8 pl-2" type="text" name="streamlink" :value="eventData."/>-->
<!--          <ErrorMessage name="streamlink" class="text-red-500" />-->
<!--        </div>-->

<!--        <div class="form-group  ml-2 mt-4 flex flex-col">-->
<!--          <label for="category" class="text-sm">Категория</label>-->
<!--          <Field as="select" class="h-8 pl-2" name="category" >-->
<!--            <option selected value="">{{eventData.categories[0]}}</option>-->
<!--            <option :value="city.id"  v-for="city in cityList" > {{city.name}} </option>-->
<!--          </Field>-->
<!--          <ErrorMessage name="city" class="text-red-500" />-->
<!--        </div>-->
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
            <button class="px-12 py-2">Подтвердить</button>
          </div>
          <div></div>
        </div>
      </div>
    </Form>
  </div>
</template>

<style scoped>

</style>