plugins {
    id("java")
    id("application")//allows Gradle to run the app
    id("org.openjfx.javafxplugin") version "0.1.0"//the official javafx plugin
}

group = "org.example"
version = "1.0-SNAPSHOT"

repositories {
    mavenCentral()
}

dependencies {
    testImplementation(platform("org.junit:junit-bom:5.10.0"))
    testImplementation("org.junit.jupiter:junit-jupiter")
    testRuntimeOnly("org.junit.platform:junit-platform-launcher")
    implementation("com.google.protobuf:protobuf-java:3.25.1")
}

//configure JavaFX
javafx {
    version = "17.0.6" // A stable version of JavaFX
    modules("javafx.controls", "javafx.graphics") // The specific UI modules we need
}

//we tell Gradle where the main app is
application {
    mainClass.set("View.MainApp") // This must match the name of the class with your public static void main
}

tasks.test {
    useJUnitPlatform()
}