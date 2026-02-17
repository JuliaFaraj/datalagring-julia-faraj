
import type { Course, UpdateCourseDto } from "../types/course";

const baseUrl = import.meta.env.VITE_API_URL;

export async function getCourses() {
  const res = await fetch(`${baseUrl}/courses`);

  if (!res.ok) {
    const text = await res.text();
    throw new Error(`GET /courses failed: ${res.status} ${text}`);
  }

  return res.json();
}



export async function updateCourse(
  courseCode: string,
  dto: UpdateCourseDto
): Promise<Course> {
  const res = await fetch(
    `${baseUrl}/courses/${encodeURIComponent(courseCode)}`,
    {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(dto),
    }
  );

  if (!res.ok) {
    const text = await res.text();

    if (res.status === 409) {
      throw new Error(
        "Kursen har ändrats av någon annan. Ladda om sidan och försök igen."
      );
    }

    throw new Error(text || `Update failed: ${res.status}`);
  }

  return (await res.json()) as Course;
}