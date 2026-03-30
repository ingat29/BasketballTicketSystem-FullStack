package Model;

public class Team extends Entity<Integer> {
    private String name;

    public Team(Integer id, String name) {
        super(id);
        this.name = name;
    }

    public String getName() { return name; }
    public void setName(String name) { this.name = name; }

//    @Override
//    public String toString() {
//        return name;
//    }
}