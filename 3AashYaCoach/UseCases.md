# Use Case Specification: 3AashYaCoach Platform

This document provides a detailed specification of the use cases for the 3AashYaCoach platform. It outlines the interactions between the actors (Trainee, Coach, Administrator) and the system.

---

## 1. Actors

*   **Trainee:** A registered user who wants to find coaches, subscribe to workout plans, and track their fitness progress.
*   **Coach:** A registered user and fitness expert who creates and manages workout plans, oversees trainee subscriptions, and manages their professional profile.
*   **Administrator:** A privileged user responsible for system oversight, user management, and content moderation.

---

## 2. Trainee Use Cases

### UC-T01: Register for an Account
- **Goal:** To create a new Trainee account on the platform.
- **Preconditions:** The user must not have an existing account with the provided email.
- **Main Flow:**
    1. User navigates to the registration page.
    2. User selects the "Trainee" role.
    3. User provides their full name, a valid email address, and a secure password.
    4. System validates the input and confirms the email is unique.
    5. System creates a new Trainee account.
    6. System confirms successful registration and directs the user to the login page.
- **Postconditions:** A new Trainee account is created and persisted in the system.

### UC-T02: Log In to the System
- **Goal:** To securely access their personal dashboard and platform features.
- **Preconditions:** The user must have a registered account.
- **Main Flow:**
    1. User navigates to the login page.
    2. User enters their registered email and password.
    3. System validates the credentials.
    4. On success, the System grants access and displays the Trainee's personalized dashboard.
- **Postconditions:** User is authenticated and has an active session.

### UC-T03: Browse and Search for Coaches
- **Goal:** To find a suitable coach based on various criteria.
- **Main Flow:**
    1. Trainee navigates to the "Find a Coach" section.
    2. System displays a list of available coaches with key information (name, rating, specialties).
    3. Trainee can filter or search the list by name or other attributes.
    4. Trainee selects a coach from the list to view their detailed profile.
- **Postconditions:** Trainee can view a list of coaches and their profiles.

### UC-T04: Subscribe to a Coach
- **Goal:** To establish a professional relationship with a coach and gain access to their workout plans.
- **Preconditions:** Trainee must be logged in.
- **Main Flow:**
    1. Trainee is viewing a Coach's profile.
    2. Trainee clicks the "Subscribe" button.
    3. System verifies that the Trainee is not already subscribed.
    4. System creates a subscription record, linking the Trainee to the Coach.
    5. System confirms the subscription and updates the UI to reflect the active status.
- **Postconditions:** Trainee is officially subscribed to the Coach.

### UC-T05: View and Follow a Workout Plan
- **Goal:** To access and interact with a subscribed workout plan.
- **Preconditions:** Trainee must be subscribed to the coach who created the plan.
- **Main Flow:**
    1. Trainee navigates to their "My Plans" or "Subscribed Plans" section.
    2. Trainee selects a workout plan to view its details (days, exercises).
    3. Trainee follows the exercises for a specific day.
    4. Trainee marks a day as "Completed" to track their progress.
- **Postconditions:** Trainee's progress on the workout plan is updated.

### UC-T06: Rate a Coach
- **Goal:** To provide feedback and a rating for a subscribed coach.
- **Preconditions:** Trainee must have an active or past subscription with the coach.
- **Main Flow:**
    1. Trainee navigates to the Coach's profile.
    2. Trainee selects a star rating (1-5) and optionally adds a text review.
    3. System validates and saves the rating.
    4. System updates the Coach's average rating.
- **Postconditions:** The Coach's rating is updated with the new feedback.

### UC-T07: Communicate with a Coach via Chat
- **Goal:** To ask questions and receive guidance from a coach.
- **Preconditions:** Trainee must be logged in and subscribed to the coach.
- **Main Flow:**
    1. Trainee opens the chat interface from the coach's profile or a dedicated chat section.
    2. System loads the conversation history.
    3. Trainee types and sends a message.
    4. System saves the message and delivers it in real-time to the Coach.
- **Postconditions:** A message is sent and stored in the chat history.

---

## 3. Coach Use Cases

### UC-C01: Create and Manage Professional Profile
- **Goal:** To build an attractive and informative profile to attract trainees.
- **Preconditions:** User must be logged in with a "Coach" role.
- **Main Flow:**
    1. Coach navigates to their profile management page.
    2. Coach adds or updates their details, such as certifications, experience, and a profile image.
    3. System validates and saves the information.
- **Postconditions:** The Coach's public profile is updated.

### UC-C02: Create a Workout Plan
- **Goal:** To design a structured workout plan for trainees.
- **Main Flow:**
    1. Coach navigates to the "Workout Plans" section and chooses to create a new plan.
    2. Coach enters the plan's name, primary goal, and sets it as public or private.
    3. Coach adds workout days (e.g., "Day 1: Chest", "Day 2: Legs").
    4. For each day, the Coach adds specific exercises with details (e.g., sets, reps, notes).
    5. System validates and saves the entire plan.
- **Postconditions:** A new workout plan is created and available for subscription.

### UC-C03: Manage Workout Plans
- **Goal:** To edit, update, or delete existing workout plans.
- **Main Flow:**
    1. Coach views their list of created plans.
    2. Coach selects a plan to either edit its details or delete it entirely.
    3. System applies the changes and confirms the action.
- **Postconditions:** The selected workout plan is updated or removed from the system.

### UC-C04: View and Manage Subscribers
- **Goal:** To see a list of all trainees who have subscribed to them.
- **Main Flow:**
    1. Coach navigates to their dashboard or a "My Subscribers" section.
    2. System displays a list of all active subscribers.
    3. Coach can select a trainee to view their profile or initiate a chat.
- **Postconditions:** Coach has an up-to-date view of their client base.

---

## 4. Administrator Use Cases

### UC-A01: Manage User Accounts
- **Goal:** To oversee all user accounts on the platform.
- **Preconditions:** User must be logged in with an "Administrator" role.
- **Main Flow:**
    1. Administrator navigates to the User Management dashboard.
    2. System displays a list of all users (Trainees, Coaches, other Admins).
    3. Administrator can perform actions such as viewing a user's profile, changing their role, disabling their account, or deleting them.
- **Postconditions:** The state or role of a user account is modified.

### UC-A02: Oversee Platform Content
- **Goal:** To moderate content to ensure it meets platform guidelines.
- **Main Flow:**
    1. Administrator views dashboards for workout plans, coach profiles, or ratings.
    2. Administrator can review any content and has the authority to edit or delete inappropriate or low-quality content (e.g., remove a spam workout plan or a fake rating).
- **Postconditions:** Platform content is moderated and maintained. 