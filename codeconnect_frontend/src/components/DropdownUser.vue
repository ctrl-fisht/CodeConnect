<script>
export default {
  name: "DropdownUser",
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
    },
    logout() {
      this.$store.dispatch("auth/logout");
      this.$emit("update:isOpen", false);
      this.$router.push("/");
    }
  }

}
</script>

<template>
  <div>
    <button @focusout="closeMenu"  @click="toggleMenu">
      <img src="@/static/avatar.png" class="size-12 cursor-pointer"/>
    </button>
    <transition name="fade">
      <div v-if="isOpen === true" class="fixed" @click.stop>
        <ul class=" bg-white py-4 px-4 rounded-xl">
          <li class="cursor-pointer"><router-link class="flex justify-center hover:text-orange-400" to="/settings">Настройки</router-link></li>
          <li><a href="#" @click="logout" class="flex justify-center hover:text-orange-400">Выйти</a></li>
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