export default {
    props: {
        show: {
            type: Boolean,
            default: false
        }
    },
    methods: {
        hide() {
            this.$emit("update:show", false)
        }
    },
    mounted() {

    }
    // ...
}