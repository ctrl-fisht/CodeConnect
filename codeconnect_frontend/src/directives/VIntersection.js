export default {
    mounted(el, binding) {
        console.log(el, binding);
        const options = {
            rootMargin: '0px',
            threshold: 1.0
        };
        const callback = (entries, observer) => {
            if (entries[0].isIntersecting)
                binding.value();
        };
        const obsverver = new IntersectionObserver(callback, options);
        obsverver.observe(el);
    },
    name: "intersection"
}