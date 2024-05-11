import {createStore} from "vuex";
import {auth} from "@/store/authModule"
import {user} from "@/store/userModule"
export default createStore({
    modules: {
        auth,
        user
    }
})