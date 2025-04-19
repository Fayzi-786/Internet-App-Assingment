const url = window.location.href
const params = new URLSearchParams(new URL(url).search)
const id  = params.get('id')
let event_detail;
function fetchJSONData() {
  return fetch("../event_data.json")
    .then((res) => {
      if (!res.ok) {
        throw new Error(`HTTP error! Status: ${res.status}`);
      }
      return res.json();
    })
    .then((data) => {
        
    event_detail =  data.events.filter(ev => ev.id === +id)
    })
    .catch((error) => console.error("Unable to fetch data:", error));
}
function renderEvent(event){
    const container = document.querySelector(".event-detail-container")
    container.innerHTML = `
             <div class="event-banner">
        <img src="../Assets/images/${event.image}" alt="Event Banner" />
        <div class="event-title-overlay">
          <h1>${event.title}</h1>
          <p>Event Subtitle or Tagline</p>
        </div>
      </div>

      <div class="event-detail-content">
        <div class="event-info">
          <h2>Event Details</h2>
          <p>
            <i class="fas fa-calendar-alt"></i> Date:
            <span>${event.date}</span>
          </p>
          <p>
            <i class="fas fa-clock"></i> Time: <span>${event.time}</span>
          </p>
          <p>
            <i class="fas fa-map-marker-alt"></i> Location:
            <span>${event.location}</span>
          </p>
          <p><i class="fas fa-ticket-alt"></i> Price: <span>$50</span></p>
        </div>
        <div class="event-description">
          <h2>About the Event</h2>
          <p>
            Join us for an unforgettable evening of entertainment, inspiration,
            and connection at the annual gala. Featuring top performers, keynote
            speakers, and much more, this event promises to be a highlight of
            the season. Reserve your spot now!
          </p>
        </div>


        <div class="event-action">
          <a href="#" class="btn-register">Register Now</a>
        </div>
      </div>    

    `
}
document.addEventListener("DOMContentLoaded", async () => {
  await fetchJSONData();
  console.log(event_detail[0])
  renderEvent(event_detail[0]); 
});
document.addEventListener('DOMContentLoaded', () => {
  const themeToggle = document.createElement('button');
  themeToggle.id = 'theme-toggle';
  themeToggle.innerHTML = `
    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
      <circle cx="12" cy="12" r="5"/>
      <path d="M12 1v2M12 21v2M4.2 4.2l1.4 1.4M18.4 18.4l1.4 1.4M1 12h2M21 12h2M4.2 19.8l1.4-1.4M18.4 5.6l1.4-1.4"/>
    </svg>
  `;
  
  document.body.appendChild(themeToggle);


 const savedTheme = localStorage.getItem('Mode');
 if (savedTheme === "dark"){document.documentElement.classList.add('dark-mode')}
 if (savedTheme === "light"){document.documentElement.classList.remove('dark-mode')}
 
 document.querySelector("#theme-toggle").addEventListener("click", () =>{
  document.documentElement.classList.toggle("dark-mode")
  const isDarkMode = document.documentElement.classList.contains("dark-mode")
  if (isDarkMode){
  localStorage.setItem("Mode", "dark")

  }else{
  localStorage.setItem("Mode", "light")
 }

 });

});


