let eventsData = [];

function fetchJSONData() {
  return fetch("../event_data.json")
    .then((res) => {
      if (!res.ok) {
        throw new Error(`HTTP error! Status: ${res.status}`);
      }
      return res.json();
    })
    .then((data) => {
      console.log("Fetched data:", data);
      eventsData = data; 
    })
    .catch((error) => console.error("Unable to fetch data:", error));
}

function createEventCard(event) {
  return `
    <div class="event-card-in-events">
        <img src="../Assets/images/${event.image}" alt="${event.title}" class="event-image-in-events">
        <div class="event-details-in-events">
            <h2 class="event-title-in-events">
                <i class="fas ${event.icon}"></i>
                ${event.title}
            </h2>
            <p class="event-date-in-events">
                <i class="fas fa-calendar-alt"></i>
                ${event.date}
            </p>
            <p class="event-description-in-events">${event.description}</p>
        </div>
        <div class="event-footer">
            <div class="event-location">
                <i class="fas fa-map-marker-alt"></i>
                ${event.location}
            </div>
            <a href="/Event_Details/Event-details.html?id=${event.id}" class="event-action">Details</a>
        </div>
    </div>
  `;
}
function renderEvents(events) {
  const container = document.getElementById("event-container-in-events");
  container.innerHTML = events.map(createEventCard).join("");
}
document.addEventListener("DOMContentLoaded", async () => {
   await fetchJSONData();
  renderEvents(eventsData.events); 
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
  const systemPrefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;

  if (savedTheme === 'dark' || (!savedTheme && systemPrefersDark)) {
    document.documentElement.classList.add('dark-mode');
  }
  themeToggle.addEventListener('click', () => {
    document.documentElement.classList.toggle('dark-mode');
    const isDarkMode = document.documentElement.classList.contains('dark-mode');
    localStorage.setItem('Mode', isDarkMode ? 'dark' : 'light');
  });
  if (savedTheme === "dark") { document.documentElement.classList.add('dark-mode') }
  if (savedTheme === "light") { document.documentElement.classList.remove('dark-mode') }
  
});
document.addEventListener('DOMContentLoaded', () => {
  
});
let searchIn = 'title';

function search(val, searchIn) {
  let updatedEvents;
  const searchValue = val.toLowerCase();
  switch (searchIn) {
    case "title":
      updatedEvents = eventsData.events.filter(a => a.title.toLowerCase().includes(searchValue));
      break;
    case "description":
      updatedEvents = eventsData.events.filter(a => a.description.toLowerCase().includes(searchValue));
      break;
    case "location":
      updatedEvents = eventsData.events.filter(a => a.location.toLowerCase().includes(searchValue));
      break;
    case "date":
      updatedEvents = eventsData.events.filter(a => a.date.toLowerCase().includes(searchValue));
      break;
    case "time":
      updatedEvents = eventsData.events.filter(a => a.time.toLowerCase().includes(searchValue));
      break;
    default:

      updatedEvents = eventsData.events.filter(a => 
        a.title.toLowerCase().includes(searchValue) ||
        a.description.toLowerCase().includes(searchValue) ||
        a.location.toLowerCase().includes(searchValue) ||
        a.date.toLowerCase().includes(searchValue) ||
        a.time.toLowerCase().includes(searchValue)
      );
      break;
  }

  renderEvents(updatedEvents); 
}
document.getElementById('search-events').addEventListener('input', e => {
  search(e.target.value, searchIn);
});

document.getElementById("filterSelect").addEventListener('change', e => {
  searchIn = e.target.value;
});
