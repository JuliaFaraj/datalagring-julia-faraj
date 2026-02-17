export type Course = {
  id: number;
  courseCode: string;
  title: string;
  description: string;
  createdAt: string;
  updatedAt: string;
  rowVersion: string;
};

export type UpdateCourseDto = {
  courseCode: string;
  title: string;
  description: string;
  rowVersion: string;
};
