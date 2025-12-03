import axios from "axios";

// ✅ יצירת axios instance עם Base URL
const api = axios.create({
  baseURL: process.env.REACT_APP_API_URL || "http://localhost:5091",
  // עכשיו זה יקח אוטומטית את הכתובת מה-.env כשאת ב-production
});
// ✅ Interceptor לתפיסת שגיאות
api.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error("API ERROR:", error);
    return Promise.reject(error);
  }
);

// ✅ שליפת כל המשימות
export const getTasks = async () => {
  const res = await api.get("/tasks");
  return res.data;
};

// ✅ הוספת משימה חדשה
export const addTask = async (name) => {
  const res = await api.post("/tasks", {
    name: name,
    isComplete: false,
  });
  return res.data;
};

// ✅ עדכון משימה (סימון V)
export const setCompleted = async (id, isComplete) => {
  const res = await api.put(`/tasks/${id}`, {
    id: id,
    isComplete: isComplete,
    name: "", // ימלא אוטומטית מהשרת אם תרצי
  });
  return res.data;
};

// ✅ מחיקת משימה
export const deleteTask = async (id) => {
  await api.delete(`/tasks/${id}`);
};
