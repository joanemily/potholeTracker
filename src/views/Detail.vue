<template>
  <div id="detail">
    <div class="pothole">
      <div id="info">
        <h1>{{pothole.location}}</h1>
        <h3>Reported On: {{formatDate(pothole.dateReported)}}</h3>
        <h3 v-if="pothole.dateInspected != null">Inspection: {{formatDate(pothole.dateInspected)}}</h3>
        <h3
          v-if="pothole.dateBeginRepair != null"
        >Start of Repairs: {{formatDate(pothole.dateBeginRepair)}}</h3>
        <h3
          v-if="pothole.dateFinishRepair != null"
        >End of Repairs: {{formatDate(pothole.dateFinishRepair)}}</h3>
        <p
          v-if="!isEmployee"
        >Hello Citizen! 
        We are so grateful for your continued diligence in helping keep your city streets safe! 
        Please know that we are always here for you. And if you have any questions or concerns, you can always reach us by phone at 1-800-NOHOLES. 
        If you would like to report a pothole, please click the link above. 
        Thank you!</p>
      </div>
      <form v-on:submit.prevent="savePothole()">
        <div class="grid">
          <div v-if="isEmployee" class="inspect">
            <div v-if="pothole.dateInspected == null">
              <label class="item label">Inspection:</label>
              <DatePick              
              v-model="pothole.date" 
              class="item input" 
              :displayFormat="'MM.DD.YYYY'"
              ></DatePick>
            </div>
            <div v-if="pothole.dateInspected != null && pothole.dateBeginRepair == null" class="info">
              <label class="item label">Severity:</label>
              <select v-model="pothole.severity" class="item">
                <option value="1">Mild</option>
                <option value="2">Less Moderate</option>
                <option value="3">Moderate</option>
                <option value="4">Less Severe</option>
                <option value="5">Severe</option>
              </select>
            </div>
          </div>

          <div v-if="pothole.dateInspected != null && isEmployee" class="repairs">
            <div v-if="pothole.dateBeginRepair == null" class="info">
              <label class="item label">Repairs Begin:</label>
              <DatePick v-model="pothole.date" class="item input" :displayFormat="'MM.DD.YYYY'"></DatePick>
            </div>
            <div v-if="pothole.dateBeginRepair != null && pothole.dateFinishRepair == null" class="info">
              <label class="item label">Repairs Finished:</label>
              <DatePick v-model="pothole.date" class="item input" :displayFormat="'MM.DD.YYYY'"></DatePick>
            </div>
          </div>
          <div class="button">
            <button type="submit" value="save" v-if="isEmployee" class="submit item">Submit</button>
            <button class="delete item" v-if="pothole.dateFinishRepair != null && isEmployee">Archive</button>
            <router-link :to="{name: 'home'}" tag="button" class="item">Cancel</router-link> 
          </div>
        </div>
          <div id="photos">
            <div id="photo" v-for="picture in pictures" v-bind:key="picture.id">
              <img v-bind:src="picture.path" />
            </div>
          </div>
          <br />
          <div id="upload">
            <button v-if="!showUpload" @click="showUpload = true">Add Photo</button>
            <div v-if="showUpload" id="showUpload">
              <upload @upload="uploadedPhoto" />
              <button type="button" v-on:click="uploadPictures">Upload</button>
              <button @click="showUpload = false">Cancel</button>
            </div>
          </div>
          <br />
          <br />

        <div id="claim">
          <router-link :to="{name: 'claim', params: {id: pothole.id}}" tag="button">Make a Claim</router-link>
        </div>
      </form>
    </div>
  </div>
</template>

<script>

import auth from "@/auth.js";
import moment from 'moment';
import uploadPhotos from '@/views/UploadPhoto.vue';
import DatePick from 'vue-date-pick';
import fecha from 'fecha'

export default {
  
  name: "detail",
  computed: {
    isEmployee() {
      let user = auth.getUser();
      if (user && user.rol === "Employee") {
        return true;
      } else {
        return false;
      }
    },
  },
  components: {
    'upload': uploadPhotos,
    DatePick,
  },
  data() {
    return {
      showUpload: false,
      showDate: true,
      pothole: {
        id: 0,
        location: "",
        dateReported: null,
        dateInspected: null,
        dateBeginRepair: null,
        dateFinishRepair: null,
        status: "",
        severity: null,
        isArchived: null,
        date: new Date(),
      },
      filter: null,
      picture: {
          pictureId: 0,
          path: "",
          potholeId: 0,
          userId: 0,
          username: auth.getUser().sub,
      },
      user: "",
      pictures: [],
      photoPath: null,
      status: null,
      format: 'YYYY-MM-DDTHH:mm:ss',
      date: new Date(),
    };
  },
  methods: {

    parseDate(dateString, format) {
        return fecha.parse(dateString, format);
    },
    loadPothole(id) {
      const url = `${process.env.VUE_APP_REMOTE_API}/potholes/detail/${id}`; 
      console.log(url);
      fetch(url)
        .then(response => {
          if (response.ok) {
            response.json().then(json => {
              this.pothole = json;
            });
          } else {
            console.log(`There was an error: ${response}.`);
          }
        })
        .catch(err => {
          console.log(err);
        })

    },

    deletePothole(id) {
      const url = `${process.env.VUE_APP_REMOTE_API}/potholes/detail/${id}`;
        fetch(url, {
          method: 'DELETE'
        })
        .then(response => {
          if (response.ok) {
          this.$router.push({name: 'Potholes', query: {Id: this.pothole.id}});
          } else {
            console.log(`There was an error: ${response}.`);
          }
        })
        .catch(err => {
          console.log(err);
        });

    },

    savePothole() {
      let url = `${process.env.VUE_APP_REMOTE_API}/potholes/detail/${this.pothole.id}`;

      if (this.pothole.repairStatus == 'Reported') {
        this.pothole.dateInspected == this.pothole.date;
      }

      else if (this.pothole.repairStatus == 'Inspected' && this.pothole.dateBeginRepair == null) {
        this.pothole.dateBeginRepair == this.pothole.date;
      }

      else if (this.pothole.repairStatus == 'Inspected' && this.pothole.dateBeginRepair != null) {
        this.pothole.dateFinishRepair == this.pothole.date;
      }

      // fetch here...
      fetch(url, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: 'Bearer ' + auth.getToken()
        },
        body: JSON.stringify(this.pothole)
        }).then(response => {
          if (response.ok) {
            alert("Pothole has been updated!");
            this.$router.push({name: 'home'})
          } else {
            alert(
              `There was an error updating: ${response.status}: ${response.statusText}`
            );
          }
        });
    },

    formatDate(date) {
      return moment(date, 'YYYY-MM-DD HH:mm:ss').format('MMMM DD, YYYY');
    },


    getPictures(potholeId) {
      let url = `${process.env.VUE_APP_REMOTE_API}/photos/load?potholeId=${potholeId}`;
      fetch(url)
        .then(response => {
          if (response.ok) {
            response.json().then(json => {
              this.pictures = json;
            });
          } else {
            console.log(`There was an error: ${response}.`);
          }
        })
        .catch(err => {
          console.log(err);
        });
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
            this.getPictures(this.pothole.id);
           
          })
          .catch(err => {
            console.error(err);
          })
    
          this.showUpload = false;
      
      },
 

      uploadedPhoto(url) {
        console.log(url);
        this.photoPath = url;
      }
  },

  
  created() {
    this.loadPothole(this.$route.params.id);
    this.getPictures(this.$route.params.id);
  }
};
</script>

<style>
.pothole {
  display: flex;
  flex-direction: column;
  justify-content: center;
  max-width: 50%;
  font-family: "Courier New", Courier, monospace;
  background-color: whitesmoke;
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  padding: 10px 0 20px 0;
  position: absolute;
  left: 50%;
  margin-right: -50%;
  transform: translate(-50%);
  padding: 10px 20px 20px 20px;
  margin-bottom: 100px;
}

h3 {
  text-align: center;
  justify-content: center;
}

h1 {
  text-align: center;
  font-family: "Courier New", Courier, monospace;
}

.grid {
  display: flex;
  flex-direction: column;
}

.button {
  display: flex;
  justify-content: center;
}

.button > button {
  width: 100px;
}

button {
  border: none;
  padding: 10px 5px 10px 5px;
  background-color: darkgray;
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  transition: 0.3s;
  text-align: center;
  font-family: "Courier New", Courier, monospace;
  border-radius: 25px;
  padding: 1% 2% 1% 2%;
  width: 200px;
}

button:hover {
  box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
}

option {
  font-family: "Courier New", Courier, monospace;
}

label {
  display: inline-block;
  width: 110px;
  text-align: right;
  font-size: small;
  white-space: nowrap;
}

input,
select {
  width: 120px;
  height: 22px;
  -moz-box-sizing: border-box;
  -webkit-box-sizing: border-box;
  box-sizing: border-box;
}

.item {
  margin: 10px;
}

.input {
  max-width: 100%;
}

.inspect {
  display: flex;
  justify-content: center;
}

.inspect > div {
  display: flex;
  flex-direction: row;
}

.repairs {
  display: flex;
  justify-content: center;
}

.repairs > div {
  display: flex;
  flex-direction: row;
}

p {
  text-align: center;
}

#photos {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
}

#photo > img {
  border: 1px solid #ddd;
  border-radius: 4px;
  padding: 5px;
  width: 300px;
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  transition: transform 0.3s ease-in-out;
  margin: 7px;
}

#photo > img:hover {
  transform: scale(1.5);
  box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
}
#upload {
  display: flex;
  flex-direction: row;
  justify-content: center;
}

#upload > button {
  white-space: nowrap;
}

#showUpload {
  display: flex;
  flex-direction: column;
  justify-content: center;
}

#showUpload > button {
  margin: 10px 0;
}

#claim {
  display:flex;
  justify-content: center;
  white-space: nowrap;
  margin-bottom: 10px;
}

@media (max-width: 500px) {

  h1 {
    font-size: large;
  }
  h3 {
    font-size: small;
  }
  img {
    max-width: 90%;
  }
  .grid {
    /*display: none;*/
  }

  #photo > img:hover {
    transform: scale(1);
  }
  .button {
    flex-direction: column;
    align-items: center;
  }
  .button > button {
    width: 100%;
  }
  button {
    padding: 5px 20px;
    width: 100%;
  }
  .info {
    text-align: center;
  }
  .item {
    display: block;
  }
  label {
    text-align: center;
  }

}


</style>