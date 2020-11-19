
<template>
  <div class="home">
    <div id="filter">
      <button v-if="isEmployee" v-on:click="filterPotholes('Reported')">Reported Potholes</button>
      <button v-if="isEmployee" v-on:click="filterPotholes('Inspected')">Inspected Potholes</button>
      <button v-if="isEmployee" v-on:click="filterPotholes('Repaired')">Repaired Potholes</button>
      <button v-if="isEmployee" v-on:click="loadPotholes">View All Potholes</button>
    </div>
    <div class="potholes">
      <router-link 
      :to="`/detail/${pothole.id}`" 
      tag="div"  
      id="pothole" 
      v-for="pothole in potholes" 
      v-bind:key="pothole.id" 
      v-bind:potholeId="pothole.id"
      v-bind:class="{archived: pothole.dateFinishRepair != null, severe: pothole.severity == 5, moderatelySevere: pothole.severity == 4, moderate: pothole.severity == 3, moderatelyMild: pothole.severity == 2, mild: pothole.severity == 1}">
        <h3 id="location">{{pothole.location}}</h3>
        <h6>Reported on {{formatDate(pothole.dateReported)}}</h6>
      </router-link>
    </div>
  </div>
</template>

<script>
import auth from "@/auth.js";
import moment from 'moment';


export default {
  name: "home",
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
  data() {
    return {
      potholes: [],
    };
  },

  methods: {
    loadPotholes() {
      const url = `${process.env.VUE_APP_REMOTE_API}/potholes`;
      console.log(url);
      fetch(url)
        .then(response => {
          if (response.ok) {
            response.json().then(json => {
              this.potholes = json;
            });
          } else {
            console.log(`There was an error: ${response}.`);
          }
        })
        .catch(err => {
          console.log(err);
        });
    },
          filterPotholes(status) {
        const url = `${process.env.VUE_APP_REMOTE_API}/potholes/filtered?status=${status}`;
        
        fetch(url)
          .then(response => {
            if (response.ok) {
              response.json().then(json => {
                this.potholes = json;
              });
            } else {
              console.log(`There was an error: ${response}.`);
            }
          })
          .catch(err => {
            console.log(err);
          });
      
    },

    formatDate(date) {
      return moment(date, 'YYYY-MM-DD HH:mm:ss').format('MMMM DD, YYYY');
    }

    
  },

  created() {
    this.loadPotholes();
  },


};
</script>

<style scoped>

button {
  display: flex;
  flex-direction: column;
  justify-content: center;
}

#pothole {
background-color: whitesmoke;
  padding: 5px;
  margin: 10px 100px;
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
  transition: 0.3s;
  text-align: center;
  font-family: "Courier New", Courier, monospace;
  min-height: 4em;
  max-height: 10em;
  border-radius: 25px;
}

@media only screen and (min-width: 1024px){
#filter {
  display: flex;
  flex-direction: row;
  justify-content: center;
}

button {
  margin: 5px;
  background-color: darkgray;
  color: white;
  border: none;
  font-family: "Courier New", Courier, monospace;
  border-radius: 25px;
}

button:hover {
  box-shadow: 0 10px 10px 0 rgba(0,0,0,0.2), 0 5px 5px 0 rgba(0,0,0,0.19);
}

.potholes {
  display: flex;
  margin-top: 10px;
  justify-content: center;
  flex-wrap: wrap;
}

#pothole {
  background-color: whitesmoke;
  padding: 5px;
  margin: 10px;
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
  transition: 0.3s;
  text-align: center;
  font-family: "Courier New", Courier, monospace;
  max-width: 8%;
  min-width: 5%;
  min-height: 4em;
  max-height: 10em;
  border-radius: 25px;
}
}

@media (max-width: 1024px) and (min-width: 401px) {
  #filter {
  display: flex;
  flex-direction: row;
  justify-content: center;
}
  .potholes {
    width: 100%;
  }
}

#pothole:hover {
  box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);  
  transform: scale(2),
}

.severe {
  border-top: red 10px solid;
}

.moderatelySevere {
  border-top: orangered 10px solid; 
}

.moderate {
  border-top: darkorange 10px solid;
}

.moderatelyMild {
  border-top: orange 10px solid;
}

.mild {
  border-top: yellow 10px solid;
}

.archived {
  border-top: slateblue 10px solid;
}

@media (max-width: 400px) {
  #filter {
    display: none;
  }

}
</style>
