<template>
  <div id="report">
    <h2>Report a Pothole</h2>
    <table class="form">
      <tr>
        <td class="text">Location of Pothole:</td>
        <td>
          <input class="input" type="text" v-model="pothole.location" />
        </td>
      </tr>
      <tr>
        <td class="text">Severity of Pothole:</td>
        <td>
          <!-- <input class="input" type="text" v-model="pothole.severity" /> -->
          <select v-model="pothole.severity"> 
            <option disabled value="">Pothole Severity</option>
            <option value="1">Mild</option>
            <option value="2">Moderately Mild</option>
            <option value="3">Moderate</option>
            <option value="4">Moderately Severe</option>
            <option value="5">Severe</option>
          </select>
        </td>
      </tr>
      <tr>
        <td><button v-on:click="addReport">Add Report</button></td>
        <td>
          <router-link :to="{name: 'home'}" tag="button">Cancel</router-link>
        </td>
      </tr>
    </table>
    <br/>
    <upload @upload="uploadedPhoto"/>
</div>
</template>

<script>
import uploadPhoto from '@/views/UploadPhoto.vue';
import auth from "@/auth.js";



export default {
  name: "report",

  components: {
    'upload': uploadPhoto,
  },

  data() {
    return {
      photoPath: null,
      pothole: {
          potholeId: 0,
          location: "",
          date: "",
          severity: "",
          status: "Reported"
      },
      picture: {
          pictureId: 0,
          name: "",
          path: "",
          potholeId: 0,
          userId: 0,
          username: auth.getUser().sub,
      },
      user: "",
    };
  },
  
  methods: {
      addReport() {

      let url = `${process.env.VUE_APP_REMOTE_API}/potholes`;
      console.log(url);

      fetch(url, {
        method: 'POST',
        headers: {
          "Content-Type" : "application/json"
        },
        body: JSON.stringify(this.pothole)
      })
      .then(response => {
        console.log(response);
        if (response.ok){
          return response.json();
        }
      })
      .then(json => {
        console.log(json);
            this.pothole = json;
            if(this.photoPath != null) {
              this.uploadPictures();
            }
            alert(`Your Pothole Report has been filed with an ID of ${this.pothole.id}`);
            this.$router.push(`/detail/${this.pothole.id}`);            
        })
      .catch(err => {
        console.error(err);
        }
        );

      },

      uploadPictures() {
        this.picture.potholeId = this.pothole.id;
        this.picture.path = this.photoPath;

      let url = `${process.env.VUE_APP_REMOTE_API}/photos/upload`;
      console.log(url);

      fetch(url, {
        method: 'POST',
        headers: {
          "Content-Type" : "application/json"
        },
        body: JSON.stringify(this.picture)
      })
      .then(response => {
        console.log(response);
        if (response.ok){
          return response.json(); 
        }
      })
      .then(json => {
        console.log(json);
            this.picture = json;           
        })
      .catch(err => {
        console.error(err);
        }
        );
      },



      uploadedPhoto(url) {
        console.log(url);
        this.photoPath = url;
      }

  },

};
</script>

<style scoped>



#report {
  display: flex;
  flex-direction: column;
  justify-content: center;
  max-width: 30%;
  font-family: "Courier New", Courier, monospace;
  background-color: whitesmoke;
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
  padding: 10px 0 20px 0;
  position: absolute;
  left: 50%;
  margin-right: -50%;
  transform: translate(-50%);
  padding: 10px 10px 20px 10px;
  text-align: center;
  margin-bottom: 100px;
}

button {
  border: none;
  padding: 5px;
  background-color: darkgray;
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
  transition: 0.3s;
  text-align: center;
  font-family: "Courier New", Courier, monospace;
  width: 80%;
}

button:hover {
    box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);  
}

.text {
  text-align: right;
}

table {
  border-spacing: 10px;
  table-layout: fixed;
  width: 100%;
}

.input {
  width: 80%;
}

</style>