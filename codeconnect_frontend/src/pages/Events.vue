<script>
import EventHorizontalCard from "@/components/EventHorizontalCard.vue"
import { Form, Field, ErrorMessage } from "vee-validate"

export default {
  name: "Events",
  components: {EventHorizontalCard, Form, Field, ErrorMessage
  },
  data() {
    return {
      loading: false,
      intersected: false,
      justScrolling: true,
      offset: 0,
      count: 5,
      eventList: [],
      eventFoundList: [],
      titleInput: '',
      tagsList: [],
      cityList: [],
      categoryList: [],
      showGone: false,
      searchName: '',
      searchCity: '',
      searchCategory: '',
      searchTags: [],
      searchCategories: [],
      searchDates: [],
      searchPast: false
    }
  },
  methods: {
    reset() {
      this.searchName = "";
      this.searchCategories = []
      this.searchCity = ""
      this.searchTags = []
      this.searchDates = []
      this.searchPast = false
      this.eventFoundList = [];
      this.intersected = false;
      this.eventList = [];
      this.offset = 0;
      this.justScrolling = true;
      this.fetchEvents();
    },
    fetchEvents() {
      this.$store.dispatch("user/getEvents", {offsetParam: 0, countParam: this.count}).then(
        data => {
          this.eventList = data;
        },
        error => {
          console.log(error);
        }
      );
    },
    addSearchEvents() {
      if (this.intersected === false)
      {
        this.intersected = true;
        return;
      }

      if (this.justScrolling)
        return;

      this.loading = true;

      this.offset += this.count;

      let input = {
        "title": null,
        "categoriesIds" : [],
        "tagsIds": [],
        "cityId": null,
        "searchDates": [],
        "past": false
      }

      if (this.searchDates.length > 0)
      {
        input.searchDates = [];
        this.searchDates.forEach((sd) => {
          if (sd !== null)
          {
            sd.setHours(12, 0, 0, 0);
            input.searchDates.push(sd.toISOString().split('T')[0]);
          }
        });
      }

      if (this.searchName !== "")
        input.title = this.searchName;

      if (this.searchCategories.length > 0)
      {
        input.categoriesIds = [];
        this.searchCategories.forEach((sc) => {
          input.categoriesIds.push(sc.categoryId);
        });
      }

      if (this.searchTags.length > 0)
      {
        input.tagsIds = [];
        this.searchTags.forEach((st) => {
          input.tagsIds.push(st.tagId);
        });
      }

      if (this.searchPast === true)
        input.searchPast = true;

      if (this.searchCity !== "")
        input.cityId = this.searchCity.cityId;


      this.$store.dispatch("user/searchEvents", {
        input, offsetParam: this.offset, countParam: this.count}).then(
          data => {
            if (data.length < this.count)
            {
              if (data.length === 0)
                this.offset -= this.count;
              else
                this.offset -= this.count % data.length;
            }
            data.forEach((d) => this.eventFoundList.push(d))
            this.loading = false;
          },
          error => {
            this.eventList = [];
            this.eventFoundList = [];
            this.loading = false;
          }
      )
    },
    addEvents() {
      if (this.intersected === false)
      {
        this.intersected = true;
        return;
      }

      if (!this.justScrolling)
        return;

      this.loading = true;

      this.offset += this.count;
      this.$store.dispatch("user/getEvents", {offsetParam: this.offset, countParam: this.count}).then(
          data => {
            if (data.length < this.count)
            {
              if (data.length === 0)
                this.offset -= this.count;
              else
                this.offset -= this.count % data.length;
            }
            data.forEach((d) => this.eventList.push(d));
            this.loading = false;
          },
          error => {
            console.log(error);
            this.loading = false;
          }
      );

    },
    handleSearch() {
      this.offset = 0;
      this.justScrolling = false;
      if (this.searchName === ""
          && this.searchCategories.length === 0
          && this.searchCity === ""
          && this.searchTags.length === 0
          && this.searchDates.length === 0
          && this.searchPast === false) {
        if (this.eventFoundList.length > 0 || this.eventList.length == 0) {
          this.eventFoundList = [];
          this.fetchEvents();
          this.justScrolling = true;
        }
        return
      }


      let input = {
        "title": null,
        "categoriesIds" : [],
        "tagsIds": [],
        "cityId": null,
        "searchDates": [],
        "past": false
      }

      if (this.searchDates.length > 0)
      {
        input.searchDates = [];
        this.searchDates.forEach((sd) => {
          if (sd !== null)
            {
              sd.setHours(12, 0, 0, 0);
              input.searchDates.push(sd.toISOString().split('T')[0]);
            }
          });
      }

      if (this.searchName !== "")
        input.title = this.searchName;

      if (this.searchCategories.length > 0)
      {
        input.categoriesIds = [];
        this.searchCategories.forEach((sc) => {
          input.categoriesIds.push(sc.categoryId);
        });
      }

      if (this.searchTags.length > 0)
      {
        input.tagsIds = [];
        this.searchTags.forEach((st) => {
          input.tagsIds.push(st.tagId);
        });
      }

      if (this.searchPast === true)
        input.searchPast = true;

      if (this.searchCity !== "")
        input.cityId = this.searchCity.cityId;


      this.$store.dispatch("user/searchEvents", {
        input, offsetParam: 0, countParam: this.count}).then(
          data => {
            this.eventList = [];
            this.eventFoundList = data;
            if (data.length < 5)
              this.intersected = false;
          },
          error => {
            this.eventList = [];
            this.eventFoundList = [];
          }
      )

    },
    fetchTagsList() {
      this.$store.dispatch("user/getTagsList").then(
          data => {
            this.tagsList = data;
          },
          error => {
            console.log(error);
          }
      )
    },
    fetchCityList() {
      this.$store.dispatch("user/getCityList").then(
          data => {
            this.cityList = data;
          },
          error => {
            console.log(error);
          }
      )
    },
    fetchCategoryList() {
      this.$store.dispatch("user/getCategoryList").then(
          data => {
            this.categoryList = data;
          },
          error => {
            console.log(error);
          }
      )
    },
  },
  mounted() {
    this.fetchEvents();
    this.fetchTagsList();
    this.fetchCityList();
    this.fetchCategoryList();
  },
}
</script>

<template>
  <main class="mb-auto mt-12 bg-white">
    <div class="container mx-auto">
      <h1 class="text-4xl">Мероприятия</h1>
      <div class="flex flex-row mt-6">
        <div class="flex flex-col basis-2/3">
          <div v-if="eventList.length === 0 && eventFoundList.length === 0" class="text-xl">Ничего не найдено</div>
          <EventHorizontalCard v-for="event in eventFoundList" :eventData="event"/>
          <EventHorizontalCard v-for="event in eventList" :eventData="event"/>
        </div>
        <div class=" shadow border border-slate-100 border-2 basis-1/3 bg-white h-fit rounded-xl ml-4" v-on:keyup.enter="handleSearch">
          <div class="ml-2  py-4 px-8">
            <div>
              <div class="form-group mt-2 flex flex-col">
                <label for="title" class="">Название</label>
                <Field name="title" type="text" v-model="searchName" class="h-9 pl-2 rounded bg-white border" @input="handleSearch"/>
              </div>
              <div class="form-group mt-4 flex flex-col">
                <label for="dateRange">Дата</label>
                <Calendar v-model="searchDates" selectionMode="range" class="h-9 rounded bg-white border"  :manualInput="false" @date-select="handleSearch" />
              </div>
              <div class="form-group mt-4 flex flex-col">
                <label for="city">Город</label>
                <Dropdown v-model="searchCity" :options="cityList" optionLabel="name" placeholder="Выберите город" class="border w-full md:w-14rem " @change="handleSearch"/>
              </div>
              <div class="form-group mt-4 flex flex-col">
                <label for="tags">Тэги</label>
                <MultiSelect name="tags" v-model="searchTags" display="chip" :options="tagsList" optionLabel="title" placeholder="Выберите тэги"
                             :maxSelectedLabels="5" class="w-full border" @change="handleSearch"/>
              </div>
              <div class="form-group mt-4 flex flex-col">
                <label for="categories">Категории</label>
                <MultiSelect name="categories" v-model="searchCategories" display="chip" :options="categoryList" optionLabel="title" placeholder="Выберите категории"
                             :maxSelectedLabels="3" class="w-full border" @change="handleSearch"/>
              </div>
              <div class="form-group mt-4 flex flex-col">
                <label for="past">Прошедшие</label>
                <ToggleButton v-model="searchPast" onLabel="Включено" offLabel="Выключено" @change="handleSearch"/>
              </div>

              <div class="mt-6 rounded cursor-pointer flex flex-row items-center justify-between">
<!--                <div>-->
<!--                  <button class="px-4 w-36 py-2 text-center self-center bg-amber-300 rounded" @click="handleSearch">-->
<!--                    <span>Применить</span>-->
<!--                  </button>-->
<!--                </div>-->
                <div>
                  <button class="px-4 w-36 py-2 text-center self-center bg-red-300 rounded" @click="reset">
                    <span>Сбросить</span>
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div role="status" v-show="loading">
      <svg aria-hidden="true" class="w-8 h-8 text-gray-200 animate-spin dark:text-gray-600 fill-blue-600" viewBox="0 0 100 101" fill="none" xmlns="http://www.w3.org/2000/svg">
        <path d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z" fill="currentColor"/>
        <path d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z" fill="currentFill"/>
      </svg>
      <span class="sr-only">Loading...</span>
    </div>
    <div class="h-20" v-intersection="addEvents" v-if="justScrolling">

    </div>
    <div class="h-20" v-intersection="addSearchEvents" v-if="!justScrolling">

    </div>
  </main>
</template>

<style scoped>

</style>