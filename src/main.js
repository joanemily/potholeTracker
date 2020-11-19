import Vue from 'vue'
import App from './App.vue'
import router from './router'
import axios from 'axios'
import VueAxios from 'vue-axios'
import Cloudinary from 'cloudinary-vue'

Vue.use(VueAxios, axios)

Vue.config.productionTip = false

new Vue({
  router,
  render: h => h(App)
}).$mount('#app'),

Vue.use(Cloudinary, {
  "cloudName": "dlobvad5p",
  "api_key": "338668862972959",
  },
);
