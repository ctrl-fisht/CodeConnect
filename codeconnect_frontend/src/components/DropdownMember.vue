<script>
import toggleMixin from "@/mixins/toggleMixin";

export default {
  name: "DropdownMember",
  props: {
    isOpen: {
      type: Boolean,
      default: false
    }
  },
  methods: {
    toggleMenu() {
      this.$emit("update:isOpen", !this.isOpen);
    },
    closeMenu() {
      this.$emit("update:isOpen", false);
    },
    openMenu() {
      this.$emit("update:isOpen", true);
    }
  },

}
</script>

<template>
  <div>
    <button type="button" class="inline-flex hover:text-orange-500" @focusout="closeMenu" @click="toggleMenu">
      Участник
      <svg class="-mr-1 w-5 text-gray-800" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true">
        <path fill-rule="evenodd" d="M5.23 7.21a.75.75 0 011.06.02L10 11.168l3.71-3.938a.75.75 0 111.08 1.04l-4.25 4.5a.75.75 0 01-1.08 0l-4.25-4.5a.75.75 0 01.02-1.06z" clip-rule="evenodd" />
      </svg>
    </button>
    <transition name="fade">
    <div v-if="isOpen === true" class="fixed" @click.stop>
      <ul class=" bg-white py-4 px-4 rounded-xl">
        <li class="cursor-pointer"><router-link to="/calendar" class="flex justify-center hover:text-orange-400">Мой календарь</router-link></li>
        <li><router-link to="/subscriptions" class="flex justify-center hover:text-orange-400">Подписки на группы</router-link></li>
      </ul>
    </div>
    </transition>
  </div>

</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

</style>