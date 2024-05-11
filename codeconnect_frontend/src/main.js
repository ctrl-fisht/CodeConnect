import { createApp } from 'vue'
import PrimeVue from 'primevue/config'
import App from './App'
import components from '@/components/UI'
import router from "@/router/router"
import store from "@/store"
import directives from "@/directives"
import Notifications from "notiwind"
import "@/index.css"

// md editor
import VMdEditor from '@kangc/v-md-editor';
import '@kangc/v-md-editor/lib/style/base-editor.css';
import githubTheme from '@kangc/v-md-editor/lib/theme/github.js';
import '@kangc/v-md-editor/lib/theme/style/github.css';
import ruRU from '@kangc/v-md-editor/lib/lang/ru-RU';
import hljs from 'highlight.js';

// md preview
import VMdPreview from '@kangc/v-md-editor/lib/preview';
import '@kangc/v-md-editor/lib/style/preview.css';



VMdEditor.lang.use('ru-RU', ruRU)
VMdEditor.use(githubTheme, {
    Hljs: hljs,
});

VMdPreview.use(githubTheme, {
    Hljs: hljs,
});

const app = createApp(App)

import MultiSelect from 'primevue/multiselect';
import Dropdown from 'primevue/dropdown';
import Calendar from 'primevue/calendar';
import Dialog from 'primevue/dialog';
import FileUpload from 'primevue/fileupload';
import Button from 'primevue/button';
import ToggleButton from 'primevue/togglebutton';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import 'primevue/resources/themes/aura-light-green/theme.css'
components.forEach(component => {
    app.component(component.name, component);
})
app.component("MultiSelect", MultiSelect);
app.component("Dropdown", Dropdown);
app.component("Calendar", Calendar);
app.component("Dialog", Dialog);
app.component("FileUpload", FileUpload);
app.component("Button", Button);
app.component("ToggleButton", ToggleButton);
app.component("DataTable", DataTable);
app.component("Column", Column);
directives.forEach(directive => {
    app.directive(directive.name, directive);
})


app
    .use(VMdEditor)
    .use(VMdPreview)
    .use(store)
    .use(router)
    .use(Notifications)
    .use(PrimeVue)
    .mount('#app')